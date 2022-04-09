using System.Collections.Generic;
using Espionage.Engine.Services;
using UnityEngine;

namespace Espionage.Engine.Overlays
{
	public class OverlayProvider : Service, IOverlayProvider
	{
		public override void OnReady()
		{
			// Tell Debugging to use this
			Debugging.Overlay = this;
		}

		public override void OnUpdate()
		{
			for ( var i = 0; i < _requests.Count; i++ )
			{
				if ( _requests[i].Draw() )
				{
					_requests.Remove( _requests[i] );
				}
			}
		}

		// Rendering

		private readonly List<Request> _requests = new();

		public void Draw( Vector3 position, Vector3 scale, Mesh mesh, float seconds, Color? color, bool depth )
		{
			// Required by IOverlayProvider

			color ??= Color.red;
			_requests.Add( new( seconds, mesh, Matrix4x4.TRS( position, Quaternion.identity, scale ), color, depth ) );
		}

		public void Draw( Matrix4x4 matrix, Mesh mesh, float seconds, Color? color, bool depth )
		{
			// Required by IOverlayProvider

			color ??= Color.red;
			_requests.Add( new( seconds, mesh, matrix, color, depth ) );
		}


		private readonly struct Request
		{
			public Request( float time, Mesh mesh, Matrix4x4 matrix, Color? color, bool depth )
			{
				Time = time;
				Mesh = mesh;
				Matrix = matrix;
				Color = color;
				Depth = depth;

				TimeSinceCreated = 0;
			}


			public bool Draw()
			{
				Graphics.DrawMesh( Mesh, Matrix, null, 0 );
				return TimeSinceCreated > Time;
			}

			private TimeSince TimeSinceCreated { get; }

			private float Time { get; }
			private Matrix4x4 Matrix { get; }
			private Mesh Mesh { get; }
			private Color? Color { get; }
			private bool Depth { get; }
		}
	}
}
