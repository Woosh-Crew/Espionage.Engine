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
		public override void OnLayout()
		{
			ImGui.Text( $"FPS: {(int)(1 / Time.smoothDeltaTime)}" );
			ImGui.Text( $"Frame: {Time.frameCount}" );
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

			ImGui.Text( $"Loading: {loader.Current.Text}" );
			ImGui.Text( $"Progress: {loader.Progress}" );
		}
	}
}
