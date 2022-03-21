using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public abstract class Window : ILibrary
	{
		private static Dictionary<Library, Window> All { get; } = new();

		internal static void Apply( DiagnosticsService service )
		{
			// I'd assume you wouldn't be able 
			// to remove more then 1 window on
			// the same frame.
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

		public static T Show<T>() where T : Window
		{
			// Gotta do this or the compiler has a fit?
			var item = Library.Database.Create<Window>( typeof( T ) );
			return item as T;
		}

		[Function( "tool.menu_bar" ), Callback( "dev.menu_bar.tools" )]
		private static void MenuBarLayout()
		{
			var menuItems = Library.Database.GetAll<Window>().Where( e => !e.Class.IsAbstract );

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
			Service = null;
			All.Remove( ClassInfo );
			Library.Unregister( this );
		}

		protected DiagnosticsService Service { get; private set; }

		internal virtual bool Layout()
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
