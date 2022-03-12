using System;
using UnityEngine;

namespace Espionage.Engine
{
	public readonly struct RealTimeSince : IEquatable<RealTimeSince>, IEquatable<float>
	{
		private RealTimeSince( float time )
		{
			_time = Time.realtimeSinceStartup - time;
		}

		private readonly float _time;

		public static implicit operator float( RealTimeSince ts )
		{
			return Time.realtimeSinceStartup - ts._time;
		}

		public static implicit operator RealTimeSince( float ts )
		{
			return new( ts );
		}

		public override bool Equals( object obj )
		{
			if ( obj is float value )
			{
				return value.Equals( Time.realtimeSinceStartup - _time );
			}

			return obj is RealTimeSince other && Equals( other );
		}

		public bool Equals( RealTimeSince other )
		{
			return _time.Equals( other._time );
		}

		public bool Equals( float other )
		{
			return (Time.realtimeSinceStartup - _time).Equals( other );
		}

		public override int GetHashCode()
		{
			return _time.GetHashCode();
		}
	}
}
