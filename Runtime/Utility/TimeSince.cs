using System;
using UnityEngine;

namespace Espionage.Engine
{
	public readonly struct TimeSince : IEquatable<TimeSince>, IEquatable<float>
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
			return new( ts );
		}

		public override bool Equals( object obj )
		{
			if ( obj is float value )
			{
				return value.Equals( Time.time - _time );
			}

			return obj is TimeSince other && Equals( other );
		}

		public bool Equals( TimeSince other )
		{
			return _time.Equals( other._time );
		}

		public bool Equals( float other )
		{
			return (Time.time - _time).Equals( other );
		}

		public override string ToString()
		{
			return $"since:{Time.time - _time}";
		}

		public override int GetHashCode()
		{
			return _time.GetHashCode();
		}
	}
}
