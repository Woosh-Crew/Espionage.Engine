#if UNITY_5_3_OR_NEWER

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	public class UnityLoggingProvider : ILoggingProvider
	{
		public Action<Entry> OnLogged { get; set; }
		public IReadOnlyCollection<Entry> All => throw new NotImplementedException();

		public void Initialize()
		{
			// Setup Unity log callback
			Application.logMessageReceived += ( message, stack, type ) =>
			{
				Add( new Entry()
				{
					Message = message,
					StackTrace = stack,

					Type = type switch
					{
						LogType.Log => Entry.Level.Info,
						LogType.Warning => Entry.Level.Warning,
						LogType.Error => Entry.Level.Error,
						LogType.Assert => Entry.Level.Exception,
						LogType.Exception => Entry.Level.Exception,

						_ => Entry.Level.Warning,
					}
				} );
			};
		}

		public void Add( Entry entry )
		{
			OnLogged?.Invoke( entry );
		}

		public void Clear()
		{
		}
	}
}

#endif
