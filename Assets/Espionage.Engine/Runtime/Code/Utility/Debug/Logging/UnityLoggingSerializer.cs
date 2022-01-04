#if UNITY_5_3_OR_NEWER

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Internal.Logging
{
	internal class UnityLoggingSerializer : ILoggingSerializer
	{
		public UnityLoggingSerializer( string path )
		{
			Path = path;
		}

		public string Path { get; set; }

		public List<Entry> Deserialize()
		{
			return JsonUtility.FromJson<List<Entry>>( Path );
		}

		public void Serialize( List<Entry> entries )
		{
			var json = JsonUtility.ToJson( entries, true );
			Debug.Log( json );
		}
	}
}

#endif
