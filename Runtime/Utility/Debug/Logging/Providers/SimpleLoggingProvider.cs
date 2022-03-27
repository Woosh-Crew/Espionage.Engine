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
						Trace = trace,
						Level = type switch
						{
							LogType.Error => "Error",
							LogType.Assert => "Assert",
							LogType.Warning => "Warning",
							LogType.Log => "Info",
							LogType.Exception => "Exception",
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

			entry.Time = DateTime.Now;

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

			Debug.Log( message.Message );
		}

		public void Clear()
		{
			_logs.Clear();
		}
	}
}
