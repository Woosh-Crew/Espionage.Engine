#if UNITY_5_3_OR_NEWER

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Internal.Logging
{
	public class UnityLoggingSerializer : ILoggingSerializer
	{
		public IEnumerable<Entry> Deserialize()
		{
			throw new NotImplementedException();
		}

		public void Serialize( IEnumerable<Entry> entries )
		{
			throw new NotImplementedException();
		}
	}
}

#endif
