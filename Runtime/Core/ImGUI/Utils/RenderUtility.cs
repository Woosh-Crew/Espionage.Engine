using Espionage.Engine.ImGUI.Assets;
using Espionage.Engine.ImGUI.Renderer;
using Espionage.Engine.ImGUI.Texture;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

#if HAS_URP
using UnityEngine.Rendering.Universal;
#elif HAS_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

namespace Espionage.Engine.ImGUI
{
	internal static class RenderUtility
	{
		public static IRenderer Create( RenderType type, ShaderResourcesAsset shaders, TextureManager textures )
		{
			// Assert.IsNotNull( shaders, "Shaders not assigned." );

			switch ( type )
			{
#if UNITY_2020_1_OR_NEWER
				case RenderType.Mesh :
					return new RendererMesh( shaders, textures );
#endif
				case RenderType.Procedural :
					return new RendererProcedural( shaders, textures );
				default :
					return null;
			}
		}

		public static CommandBuffer GetCommandBuffer( string name )
		{
#if HAS_URP || HAS_HDRP
			return CommandBufferPool.Get(name);
#else
			return new() { name = name };
#endif
		}

		public static void ReleaseCommandBuffer( CommandBuffer commandBuffer )
		{
#if HAS_URP || HAS_HDRP
			CommandBufferPool.Release(commandBuffer);
#else
			commandBuffer.Release();
#endif
		}
	}
}
