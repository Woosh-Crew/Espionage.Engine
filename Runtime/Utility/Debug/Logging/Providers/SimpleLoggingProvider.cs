using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Logging
{
	internal class SimpleLoggingProvider : ILoggingProvider
	{
		public Action<Entry> OnLogged { get; set; }
		public IReadOnlyCollection<Entry> All => _logs;

		private List<Entry> _logs = new();

		public SimpleLoggingProvider()
		{
			if ( !Application.isEditor )
			{
				Application.logMessageReceived += ( condition, trace, type ) =>
				{
					Add( new()
					{
						Message = condition,
						StackTrace = trace,
						Type = type switch
						{
							LogType.Error => Entry.Level.Error,
							LogType.Assert => Entry.Level.Error,
							LogType.Warning => Entry.Level.Warning,
							LogType.Log => Entry.Level.Info,
							LogType.Exception => Entry.Level.Error,
							_ => throw new ArgumentOutOfRangeException( nameof( type ), type, null )
						}
					} );
				};
			}
		}

		// Logs

		public void Add( Entry entry )
		{
			if ( string.IsNullOrEmpty( entry.Message ) )
			{
				return;
			}

			_logs.Add( entry );
			OnLogged?.Invoke( entry );

			OutputUnity( entry );
		}

		private void OutputUnity( Entry message )
		{
			if ( !Application.isEditor )
			{
				return;
			}

			switch ( message.Type )
			{
				case Entry.Level.Debug :
				case Entry.Level.Info :
					Debug.Log( message.Message );
					break;

				case Entry.Level.Warning :
					Debug.LogWarning( message.Message );
					break;

				case Entry.Level.Error :
					Debug.LogError( message.Message );
					break;

				case Entry.Level.Exception :
					Debug.LogError( message.Message );
					break;
				default :
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Clear()
		{
			_logs.Clear();
		}
	}
}
