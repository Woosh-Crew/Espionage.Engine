using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

using Editor = UnityEditor.Editor;

namespace Espionage.Engine.Editor
{
	[Manager( nameof( Initialize ) )]
	public abstract class Tool : ILibrary, ICallbacks, IDisposable
	{
		//
		// System
		//

		private static void Initialize()
		{
			_all ??= new List<Tool>();
			_all.Clear();

			// Use reflection get all tools
			foreach ( var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany( e => e.GetTypes().Where( e => e.IsSubclassOf( typeof( Tool ) ) ) ) )
			{
				var tool = Activator.CreateInstance( type ) as Tool;
				_all.Add( tool );
			}

			// Sort by their order
			_all = _all.OrderBy( e => e.ClassInfo.Order ).ToList();

			// Set the first too
			SetTool( All.FirstOrDefault() );
		}

		private static Tool _active;
		public static Tool Active => _active;

		public static void SetTool( Tool newTool )
		{
			if ( newTool == _active )
				return;

			_active?.End();
			_active = newTool;
			_active?.Begin();

			Callback.Run( "editor.new_tool", newTool );
		}

		private static List<Tool> _all;
		public static IReadOnlyList<Tool> All => _all;

		public static T GetTool<T>() where T : Tool
		{
			return null;
		}

		//
		// Logic
		//

		public Tool()
		{
			ClassInfo = Library.Accessor.Get( GetType() );
			Callback.Register( this );
		}

		~Tool()
		{
			Callback.Unregister( this );
		}

		public void Dispose()
		{
			Callback.Unregister( this );
		}



		public Library ClassInfo { get; private set; }

		public virtual void Begin() { }
		public virtual void End() { }
	}
}
