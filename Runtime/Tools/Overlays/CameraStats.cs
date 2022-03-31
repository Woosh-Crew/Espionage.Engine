using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
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
}
