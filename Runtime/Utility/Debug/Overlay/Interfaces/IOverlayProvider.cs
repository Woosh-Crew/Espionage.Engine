using Espionage.Engine;
using Espionage.Engine.Overlays;
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

//
// Extensions
//

public static class OverlayProviderExtensions
{
	public static void Primitive( this IOverlayProvider provider, Vector3 position, Vector3 scale, PrimitiveType primitive, float seconds, Color? color = null, bool depth = false )
	{
		provider.Draw( position, scale, Primitives.GetMesh( primitive ), seconds, color, depth );
	}
}
