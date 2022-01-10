// Makes sure we are actually in Unity
#if UNITY_STANDALONE || UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Internal.Logging
{
	internal class UnityLoggingProvider : ILoggingProvider
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
