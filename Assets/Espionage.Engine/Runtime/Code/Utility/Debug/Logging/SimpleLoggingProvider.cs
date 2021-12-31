using System;
using System.Collections.Generic;

namespace Espionage.Engine.Internal
{
	public class SimpleLoggingProvider : ILoggingProvider
	{
		public Action<Entry> OnLogged { get; set; }

		public IReadOnlyCollection<Entry> All => throw new NotImplementedException();

		public void Add( Entry entry )
		{
			UnityEngine.Debug.Log( entry.Message );
		}

		public void Clear()
		{
		}
	}
}
