using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class TimeStats : Overlay
	{
		public override void OnLayout()
		{
			ImGui.Text( $"Time: {(int)Time.time}" );
			ImGui.Text( $"Time Scale: {Time.timeScale}" );
		}
	}

	public class FrameStats : Overlay
	{
		private readonly Queue<float> _fps = new( 20 );
		private int _lastFrame;
		private TimeSince _timeSinceUpdate = 0;

		private int _low;
		private int _top;

		public override void OnLayout()
		{
			// Using a queue is stupid, but i dont care

			if ( _timeSinceUpdate > 0.1f )
			{
				_timeSinceUpdate = 0;

				var value = 1 / Time.smoothDeltaTime;

				_fps.Enqueue( value );
				_lastFrame = (int)value;

				if ( value > _top )
				{
					_top = (int)value;
				}

				if ( value < _low || _low == 0 )
				{
					_low = (int)value;
				}

				if ( _fps.Count > 20 )
				{
					_fps.Dequeue();
				}
			}

			ImGui.Text( $"FPS: {_lastFrame}" );

			if ( _fps.Count > 0 )
			{
				ImGui.SetNextItemWidth( 128 );
				var values = _fps.ToArray();
				ImGui.PlotLines( string.Empty, ref values[0], _fps.Count - 1, 0, string.Empty, _low, _top, new( 0, 32 ) );
			}

		}
	}

	public class CameraStats : Overlay
	{
		private readonly Camera _camera;

		public CameraStats()
		{
			_camera = Engine.Camera;
		}

		public override void OnLayout()
		{
			ImGui.Text( $"Position: {_camera.transform.position}" );
			ImGui.Text( $"Rotation: {_camera.transform.rotation}" );
			ImGui.Text( $"Field Of View: {(int)_camera.fieldOfView}" );
		}
	}

	public class LoaderStats : Overlay
	{
		public override void OnLayout()
		{
			var loader = Engine.Game.Loader;

			if ( loader.Current == null )
			{
				ImGui.Text( "Nothing is being loaded." );

				// Last Loaded
				if ( loader.Timing != null )
				{
					ImGui.Separator();
					var time = loader.Timing.Elapsed.Seconds > 0 ? $"{loader.Timing.Elapsed.TotalSeconds} seconds" : $"{loader.Timing.Elapsed.TotalMilliseconds} ms";
					ImGui.Text( $"Load Time: {time}" );
				}

				return;
			}

			ImGui.Text( $"Text: {loader.Current.Text}" );
			ImGui.ProgressBar( loader.Current.Progress, new( 0, 0 ) );
		}
	}
}
