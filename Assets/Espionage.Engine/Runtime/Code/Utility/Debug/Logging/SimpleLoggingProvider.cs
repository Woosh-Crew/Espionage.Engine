using System;
using System.Collections.Generic;

namespace Espionage.Engine.Internal.Logging
{
	public class SimpleLoggingProvider : ILoggingProvider
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

		public void Clear()
		{
		}
	}
}
