using UnityEditor;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( World ) )]
	public class WorldEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}
