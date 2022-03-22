using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class FrameStats : Overlay
	{
		public override void OnLayout()
		{
			ImGui.Text( $"Time: {Time.time}" );
			ImGui.Text( $"Time Scale: {Time.timeScale}" );
		}
	}

	public class TimeStats : Overlay
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
			ImGui.Text( $"Rotation: {_camera.transform.rotation}" );
			ImGui.Text( $"Field Of View: {(int)_camera.fieldOfView}" );
		}
	}
}
