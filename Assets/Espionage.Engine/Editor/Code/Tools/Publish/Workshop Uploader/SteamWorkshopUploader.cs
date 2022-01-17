using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Espionage.Engine.Editor.Internal
{
	[Library( "tool.workshop_uploader", Title = "Workshop Uploader", Help = "Publish content to the Steam Workshop", Group = "Publish" )]
	[Icon( EditorIcons.Dashboard )]
	[HelpURL( "https://github.com/Woosh-Crew/Espionage.Engine/wiki" )]
	public class SteamWorkshopUploader : Tool
	{
		[MenuItem( "Tools/Workshop Uploader _F6", false, -150 )]
		private static void ShowEditor()
		{
			var wind = GetWindow<SteamWorkshopUploader>();
		}
	}
}
