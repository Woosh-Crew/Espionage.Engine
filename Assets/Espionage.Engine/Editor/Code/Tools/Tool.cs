using System;
using System.Collections.Generic;
using UnityEditor;

using Editor = UnityEditor.Editor;

namespace Espionage.Engine.Editor
{
	public abstract class Tool : ILibrary, ICallbacks, IDisposable
	{
		//
		// System
		//

		private static Tool _active;
		public static Tool Active => _active;

		public static void SetTool( Tool newTool )
		{
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
