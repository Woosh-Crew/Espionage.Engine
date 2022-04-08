using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Services;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	[Group( "Windows" )]
	public abstract class Window : ILibrary
	{
		internal static Dictionary<Library, Window> All { get; } = new();
		private static Queue<Window> Buffer { get; } = new();

		private static bool _running;

		internal static void Apply( Diagnostics service )
		{
			_running = true;

			for ( var i = 0; i < Buffer.Count; i++ )
			{
				var value = Buffer.Dequeue();
				All.Add( value.ClassInfo, value );
			}

			// This is bad..
			Overlay.offset = 0;
			Overlay.index = 0;

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

			_running = false;
		}

		public static T Show<T>() where T : Window
		{
			var lib = Library.Database[typeof( T )];

			if ( All.ContainsKey( lib ) )
			{
				var window = All[lib] as T;
				return window;
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

			if ( !_running )
			{
				All.Add( ClassInfo, this );
			}
			else
			{
				Buffer.Enqueue( this );
			}
		}

		public void Delete()
		{
			Service = null;
			All.Remove( ClassInfo );
			Library.Unregister( this );
		}

		protected Diagnostics Service { get; private set; }
		public virtual ImGuiWindowFlags Flags => ImGuiWindowFlags.NoSavedSettings;

		internal virtual bool Layout()
		{
			if ( !Service.Enabled )
			{
				return false;
			}

			var delete = true;

			ImGui.SetNextWindowSize( new( 512, 512 ), ImGuiCond.Once );

			if ( ImGui.Begin( ClassInfo.Title, ref delete, Flags ) )
			{
				OnLayout();
			}

			ImGui.End();

			return !delete;
		}

		public abstract void OnLayout();
	}
}
