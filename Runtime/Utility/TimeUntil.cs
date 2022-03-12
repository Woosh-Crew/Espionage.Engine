using System;
using UnityEngine;

namespace Espionage.Engine
{
	public struct TimeUntil : IEquatable<TimeUntil>, IEquatable<float>
	{
		private TimeUntil( float time )
		{
			_time = Time.time + time;
		}

		private readonly float _time;

		public static implicit operator float( TimeUntil ts )
		{
			return Time.time - ts._time;
		}

		public static implicit operator TimeUntil( float ts )
		{
			return new( ts );
		}

		public override bool Equals( object obj )
		{
			if ( obj is float value )
			{
				return value.Equals( Time.time - _time );
			}

			return obj is TimeUntil other && Equals( other );
		}

		public bool Equals( TimeUntil other )
		{
			return _time.Equals( other._time );
		}

		public bool Equals( float other )
		{
			return (Time.time + _time).Equals( other );
		}

		public override int GetHashCode()
		{
			return _time.GetHashCode();
		}
	}
}
