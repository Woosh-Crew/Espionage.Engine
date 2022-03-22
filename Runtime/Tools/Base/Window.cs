using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	[Group( "Windows" )]
	public abstract class Window : ILibrary
	{
		internal static Dictionary<Library, Window> All { get; } = new();

		internal static void Apply( DiagnosticsService service )
		{
			// This is bad..
			Overlay.Offset = 0;
			Overlay.Index = 0;

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
			var lib = Library.Database[typeof( T )];

			if ( All.ContainsKey( lib ) )
			{
				return All[lib] as T;
			}

			// Gotta do this or the compiler has a fit?
			var item = Library.Database.Create<Window>( lib );
			return item as T;
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
			if ( !Service.Enabled )
			{
				return false;
			}

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
