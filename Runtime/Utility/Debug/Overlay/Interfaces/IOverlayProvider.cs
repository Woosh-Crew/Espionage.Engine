using UnityEngine;

namespace Espionage.Engine.Overlays
{
	public interface IOverlayProvider
	{
		void Draw( Vector3 position, Vector3 scale, Mesh mesh, float seconds, Color? color = null, bool depth = false )
		{
			Draw( Matrix4x4.TRS( position, Quaternion.identity, scale ), mesh, seconds, color, depth );
		}

		void Draw( Matrix4x4 matrix, Mesh mesh, float seconds, Color? color = null, bool depth = false );
	}
}
