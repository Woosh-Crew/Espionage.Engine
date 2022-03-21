using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine.Rendering.PostProcessing;

namespace Espionage.Engine.Tools
{
	public abstract class Tool : ILibrary
	{
		internal static Dictionary<Library, Tool> All { get; } = new();

		[Function( "tool.layout" ), Callback( "imgui.layout" )]
		internal static void DoLayout()
		{
			foreach ( var tool in All )
			{
				tool.Value.Layout();
			}
		}

		[Function( "tool.menu_bar" ), Callback( "dev.menu_bar.tools" )]
		internal static void MenuBarLayout()
		{
			var menuItems = Library.Database.GetAll<Tool>();

			// Post Processing Debug Overlays
			foreach ( var value in menuItems )
			{
				if ( ImGui.MenuItem( value.Title, null, All.ContainsKey( value ) ) )
				{
					if ( All.ContainsKey( value ) )
					{
						All[value].Delete();
						continue;
					}
					
					Library.Create( value );
				}
			}
		}

		// Instance

		public Library ClassInfo { get; }

		public Tool()
		{
			ClassInfo = Library.Register( this );
			All.Add( ClassInfo, this );
		}

		public void Delete()
		{
			All.Remove( ClassInfo );
			Library.Unregister( this );
		}

		private bool _open;

		internal void Layout()
		{
			if ( !ImGui.Begin( ClassInfo.Title, ImGuiWindowFlags.NoSavedSettings ) )
			{
				ImGui.End();
			}
			
			OnLayout();
		}

		public abstract void OnLayout();
	}
}
