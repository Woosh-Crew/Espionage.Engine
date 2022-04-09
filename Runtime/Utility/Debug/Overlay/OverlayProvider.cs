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
			base.OnUpdate();
		}

		// Rendering

		public void Draw( Vector3 position, Vector3 scale, Mesh mesh, float seconds, Color? color, bool depth )
		{
			// Required by IOverlayProvider
		}


		private struct Request
		{
			public Request( float time, Matrix4x4 matrix, Color? color, bool depth )
			{
				_time = time;
				_matrix = matrix;
				_color = color;
				_depth = depth;

				_timeSinceCreated = 0;
			}

			
			public bool Draw()
			{
				
			}

			private TimeSince _timeSinceCreated { get; }
			
			private float _time { get; }
			private Matrix4x4 _matrix { get; }
			private Color? _color { get; }
			private bool _depth { get; }
		}
	}
}
