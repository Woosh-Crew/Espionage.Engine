using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Internal.Logging
{
	internal class SimpleLoggingProvider : ILoggingProvider
	{
		public Action<Entry> OnLogged { get; set; }

		public IReadOnlyCollection<Entry> All => throw new NotImplementedException();

		public void Add( Entry entry )
		{
			if ( string.IsNullOrEmpty( entry.Message ) )
				return;

			switch ( entry.Type )
			{
				case Entry.Level.Info:
					UnityEngine.Debug.Log( entry.Message );
					break;

				case Entry.Level.Warning:
					UnityEngine.Debug.LogWarning( entry.Message );
					break;

				case Entry.Level.Error:
					UnityEngine.Debug.LogError( entry.Message );
					break;

				case Entry.Level.Exception:
					UnityEngine.Debug.LogError( entry.Message );
					break;
			}

		}

#if UNITY_EDITOR
		static MethodInfo _clearConsoleMethod;
		static MethodInfo clearConsoleMethod
		{
			get
			{
				if ( _clearConsoleMethod == null )
				{
					Assembly assembly = Assembly.GetAssembly( typeof( SceneView ) );
					Type logEntries = assembly.GetType( "UnityEditor.LogEntries" );
					_clearConsoleMethod = logEntries.GetMethod( "Clear" );
				}
				return _clearConsoleMethod;
			}
		}
#endif

		public void Clear()
		{
#if UNITY_EDITOR
			clearConsoleMethod.Invoke( new object(), null );
#endif
		}
	}
}
