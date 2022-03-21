using System.Collections.Generic;
using Espionage.Engine.Services;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public abstract class Window : ILibrary
	{
		internal static Dictionary<Library, Window> All { get; } = new();

		internal static void DoLayout( DiagnosticsService service )
		{
			Library toRemove = null;

			foreach ( var (key, value) in All )
			{
				value.Service = service;
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
			var menuItems = Library.Database.GetAll<Window>();

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

		public Window()
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

		protected DiagnosticsService Service { get; private set; }

		public abstract void OnLayout();
	}
}
