using System;
using UnityEngine;

namespace Espionage.Engine.Activators
{
	public readonly struct Output
	{
		public Function Function { get; }
		public object Target { get; }

		public void Invoke()
		{
			Function.Invoke( Target, null );
		}
	}

	public readonly struct Output<T>
	{
		public Function Function { get; }
		public object Target { get; }

		public void Invoke( T value )
		{
			Function.Invoke( Target, value );
		}
	}

	public readonly struct Output<T1, T2>
	{
		public Function Function { get; }
		public object Target { get; }

		public void Invoke( T1 value1, T2 value2 )
		{
			Function.Invoke( Target, value1, value2 );
		}
	}
}
