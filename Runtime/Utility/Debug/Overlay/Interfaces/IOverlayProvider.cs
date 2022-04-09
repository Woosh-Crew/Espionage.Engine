using UnityEngine;

namespace Espionage.Engine.Overlays
{
	public interface IOverlayProvider
	{
		void Draw( Vector3 position, Vector3 scale, Mesh mesh, float seconds, Color? color, bool depth );
	}
}
