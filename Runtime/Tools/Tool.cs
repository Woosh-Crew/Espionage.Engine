using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public abstract class Tool : ILibrary
	{
		internal static Dictionary<Library, Tool> All { get; } = new();

		[Function( "tool.layout" ), Callback( "imgui.layout" )]
		internal static void DoLayout()
		{
			Library toRemove = null;

			foreach ( var (key, value) in All )
			{
				if ( value.Layout() )
				{
					toRemove = key;
				}
			}

			if ( toRemove != null )
			{
				All[toRemove].Delete();
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

		internal bool Layout()
		{
			var delete = true;

			ImGui.SetNextWindowSize( new( 256, 356 ), ImGuiCond.Once );
			if ( ImGui.Begin( ClassInfo.Title, ref delete, ImGuiWindowFlags.NoSavedSettings ) )
			{
				OnLayout();
			}

			ImGui.End();

			return !delete;
		}

		public abstract void OnLayout();
	}
}
