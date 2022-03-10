using UnityEngine;

namespace Espionage.Engine
{
	public interface IDebugOverlayProvider
	{
		bool Show { get; set; }

		void Text( Vector2 pos, string text );
		void Text( Vector3 pos, string text );

		void Box( Rect rect, string text );
	}
}
