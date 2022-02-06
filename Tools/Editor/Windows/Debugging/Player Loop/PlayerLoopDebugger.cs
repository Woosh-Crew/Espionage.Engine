using System.Text;
using Espionage.Engine.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

namespace Espionage.Engine.Tools.Editor
{
	public class PlayerLoopDebugger : EditorTool
	{
		protected override void OnCreateGUI()
		{
			var scroll = new ScrollView();
			var loop = PlayerLoop.GetCurrentPlayerLoop();
			ShowSystems(scroll.contentContainer, loop.subSystemList, 0);
			
			rootVisualElement.Add( scroll );
		}


		static void ShowSystems(VisualElement root, PlayerLoopSystem[] systems, int indent)
		{
			foreach (var playerLoopSystem in systems)
			{
				if (playerLoopSystem.subSystemList != null)
				{
					var foldout = new Foldout{ text = playerLoopSystem.type.Name, style = { marginLeft = indent * 15}};
					root.Add(foldout);
					ShowSystems(foldout, playerLoopSystem.subSystemList, indent + 1);
				}
				else
				{
					root.Add(new Label(playerLoopSystem.type.Name) { style = {  marginLeft = indent * 15 } });
				}
			}
		}
	}
}
