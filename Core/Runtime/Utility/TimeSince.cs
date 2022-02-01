using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public struct TimeSince
	{
		private TimeSince( float time )
		{
			_time = Time.time - time;
		}
		
		private readonly float _time;

		public static implicit operator float( TimeSince ts )
		{
			return Time.time - ts._time;
		}

		public static implicit operator TimeSince( float ts )
		{
			return new TimeSince( ts );
		}
	}
}
