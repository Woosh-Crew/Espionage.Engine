#if UNITY_2020_1_OR_NEWER
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Espionage.Engine.ImGUI.Assets;
using Espionage.Engine.ImGUI.Texture;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Espionage.Engine.ImGUI.Renderer
{
	internal sealed class RendererMesh : IRenderer
	{
		// Skip all checks and validation when updating the mesh.
		private const MeshUpdateFlags NoMeshChecks = MeshUpdateFlags.DontNotifyMeshUsers |
		                                             MeshUpdateFlags.DontRecalculateBounds |
		                                             MeshUpdateFlags.DontResetBoneBounds |
		                                             MeshUpdateFlags.DontValidateIndices;

		// Color sent with TexCoord1 semantics because otherwise Color attribute would be reordered to come before UVs.
		private static readonly VertexAttributeDescriptor[] _vertexAttributes = new[]
		{
			new VertexAttributeDescriptor( VertexAttribute.Position, VertexAttributeFormat.Float32, 2 ), // Position.
			new VertexAttributeDescriptor( VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2 ), // UV.
			new VertexAttributeDescriptor( VertexAttribute.TexCoord1, VertexAttributeFormat.UInt32, 1 ) // Color.
		};

		private Material _material;
		private Mesh _mesh;

		private readonly Shader _shader;
		private readonly int _textureID;
		private readonly TextureManager _textureManager;
		private readonly MaterialPropertyBlock _materialProperties;

		private int _prevSubMeshCount = 1; // number of sub meshes used previously

		public RendererMesh( ShaderResourcesAsset resources, TextureManager texManager )
		{
			_shader = resources.Shader.Mesh;
			_textureManager = texManager;
			_textureID = Shader.PropertyToID( resources.PropertyNames.Texture );
			_materialProperties = new();
		}

		public void Initialize( ImGuiIOPtr io )
		{
			io.SetBackendRendererName( "Unity Mesh" );
			// Supports ImDrawCmd::VtxOffset to output large meshes while still using 16-bits indices.
			io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

			_material = new( _shader ) { hideFlags = HideFlags.HideAndDontSave & ~HideFlags.DontUnloadUnusedAsset };

			_mesh = new() { name = "DearImGui Mesh" };
			_mesh.MarkDynamic();
		}

		public void Shutdown( ImGuiIOPtr io )
		{
			io.SetBackendRendererName( null );

			if ( _mesh != null )
			{
				Object.Destroy( _mesh );
				_mesh = null;
			}

			if ( _material != null )
			{
				Object.Destroy( _material );
				_material = null;
			}
		}

		public void RenderDrawLists( CommandBuffer commandBuffer, ImDrawDataPtr drawData )
		{
			var fbOSize = drawData.DisplaySize * drawData.FramebufferScale;

			// Avoid rendering when minimized.
			if ( fbOSize.x <= 0f || fbOSize.y <= 0f || drawData.TotalVtxCount == 0 )
			{
				return;
			}

			Constants.UpdateMeshMarker.Begin();
			UpdateMesh( drawData );
			Constants.UpdateMeshMarker.End();

			commandBuffer.BeginSample( Constants.ExecuteDrawCommandsMarker );
			Constants.CreateDrawCommandsMarker.Begin();

			CreateDrawCommands( commandBuffer, drawData, fbOSize );

			Constants.CreateDrawCommandsMarker.End();
			commandBuffer.EndSample( Constants.ExecuteDrawCommandsMarker );
		}

		private void UpdateMesh( ImDrawDataPtr drawData )
		{
			// Number of submeshes is the same as the nr of ImDrawCmd.
			var subMeshCount = 0;
			for ( int n = 0, nMax = drawData.CmdListsCount; n < nMax; ++n )
			{
				subMeshCount += drawData.CmdListsRange[n].CmdBuffer.Size;
			}

			if ( _prevSubMeshCount != subMeshCount )
			{
				// Occasionally crashes when changing subMeshCount without clearing first.
				_mesh.Clear( true );
				_mesh.subMeshCount = _prevSubMeshCount = subMeshCount;
			}

			_mesh.SetVertexBufferParams( drawData.TotalVtxCount, _vertexAttributes );
			_mesh.SetIndexBufferParams( drawData.TotalIdxCount, IndexFormat.UInt16 );

			//  Upload data into mesh.
			var vtxOf = 0;
			var idxOf = 0;
			var descriptors = new List<SubMeshDescriptor>();

			for ( int n = 0, nMax = drawData.CmdListsCount; n < nMax; ++n )
			{
				var drawList = drawData.CmdListsRange[n];

				unsafe
				{
					// TODO: Convert NativeArray to C# array or list (remove collections).
					var vtxArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ImDrawVert>(
						(void*)drawList.VtxBuffer.Data, drawList.VtxBuffer.Size, Allocator.None );
					var idxArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ushort>(
						(void*)drawList.IdxBuffer.Data, drawList.IdxBuffer.Size, Allocator.None );

#if ENABLE_UNITY_COLLECTIONS_CHECKS
					NativeArrayUnsafeUtility
						.SetAtomicSafetyHandle( ref vtxArray, AtomicSafetyHandle.GetTempMemoryHandle() );
					NativeArrayUnsafeUtility
						.SetAtomicSafetyHandle( ref idxArray, AtomicSafetyHandle.GetTempMemoryHandle() );
#endif
					// Upload vertex/index data.
					_mesh.SetVertexBufferData( vtxArray, 0, vtxOf, vtxArray.Length, 0, NoMeshChecks );
					_mesh.SetIndexBufferData( idxArray, 0, idxOf, idxArray.Length, NoMeshChecks );

					// Define subMeshes.
					for ( int i = 0, iMax = drawList.CmdBuffer.Size; i < iMax; ++i )
					{
						var cmd = drawList.CmdBuffer[i];
						var descriptor = new SubMeshDescriptor
						{
							topology = MeshTopology.Triangles,
							indexStart = idxOf + (int)cmd.IdxOffset,
							indexCount = (int)cmd.ElemCount,
							baseVertex = vtxOf + (int)cmd.VtxOffset
						};
						descriptors.Add( descriptor );
					}

					vtxOf += vtxArray.Length;
					idxOf += idxArray.Length;
				}
			}

			_mesh.SetSubMeshes( descriptors, NoMeshChecks );
			_mesh.UploadMeshData( false );
		}

		private void CreateDrawCommands( CommandBuffer commandBuffer, ImDrawDataPtr drawData, Vector2 fbSize )
		{
			var prevTextureId = IntPtr.Zero;
			var clipOffset = new Vector4( drawData.DisplayPos.x, drawData.DisplayPos.y,
				drawData.DisplayPos.x, drawData.DisplayPos.y );
			var clipScale = new Vector4( drawData.FramebufferScale.x, drawData.FramebufferScale.y,
				drawData.FramebufferScale.x, drawData.FramebufferScale.y );

			commandBuffer.SetViewport( new( 0f, 0f, fbSize.x, fbSize.y ) );
			commandBuffer.SetViewProjectionMatrices(
				Matrix4x4.Translate( new( 0.5f / fbSize.x, 0.5f / fbSize.y, 0f ) ), // Small adjustment to improve text.
				Matrix4x4.Ortho( 0f, fbSize.x, fbSize.y, 0f, 0f, 1f ) );

			var subOf = 0;
			for ( int n = 0, nMax = drawData.CmdListsCount; n < nMax; ++n )
			{
				var drawList = drawData.CmdListsRange[n];
				for ( int i = 0, iMax = drawList.CmdBuffer.Size; i < iMax; ++i, ++subOf )
				{
					var drawCmd = drawList.CmdBuffer[i];
					if ( drawCmd.UserCallback != IntPtr.Zero )
					{
						var userDrawCallback = Marshal.GetDelegateForFunctionPointer<UserDrawCallback>( drawCmd.UserCallback );
						userDrawCallback( drawList, drawCmd );
					}
					else
					{
						// Project scissor rectangle into framebuffer space and skip if fully outside.
						var clipSize = drawCmd.ClipRect - clipOffset;
						var clip = Vector4.Scale( clipSize, clipScale );

						if ( clip.x >= fbSize.x || clip.y >= fbSize.y || clip.z < 0f || clip.w < 0f )
						{
							continue;
						}

						if ( prevTextureId != drawCmd.TextureId )
						{
							prevTextureId = drawCmd.TextureId;

							// TODO: Implement ImDrawCmdPtr.GetTexID().
							var hasTexture = _textureManager.TryGetTexture( prevTextureId, out var texture );
							Assert.IsFalse( hasTexture, $"Texture {prevTextureId} does not exist. Try to use UImGuiUtility.GetTextureID()." );

							_materialProperties.SetTexture( _textureID, texture );
						}

						commandBuffer.EnableScissorRect( new( clip.x, fbSize.y - clip.w, clip.z - clip.x, clip.w - clip.y ) ); // Invert y.
						commandBuffer.DrawMesh( _mesh, Matrix4x4.identity, _material, subOf, -1, _materialProperties );
					}
				}
			}

			commandBuffer.DisableScissorRect();
		}
	}
}
#endif
