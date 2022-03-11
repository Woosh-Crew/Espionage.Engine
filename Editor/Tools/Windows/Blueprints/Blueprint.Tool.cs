using UnityEditor;

namespace Espionage.Engine.Tools.Editor
{
	[Title( "Blueprint Editor" )]
	public class BlueprintTool : EditorTool
	{
		[MenuItem( "Tools/Espionage.Engine/Blueprint Editor", priority = -15 )]
		private static void ShowEditor()
		{
			GetWindow<BlueprintTool>();
		}
	}
}
