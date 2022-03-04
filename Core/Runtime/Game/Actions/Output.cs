using System;
using UnityEngine;

namespace Espionage.Engine.Activators
{
	public struct Output
	{
		public string function;
		public Behaviour target;

		public void Invoke()
		{
			target.ClassInfo.Functions[function]?.Invoke( target );
		}
	}


	[Serializable]
	public struct Output<T>
	{
		public string function;
		public Behaviour target;

		public void Invoke( T value )
		{
			target.ClassInfo.Functions[function]?.Invoke( target, value );
		}
	}

	public struct Output<T1, T2>
	{
		public string function;
		public Behaviour target;

		public void Invoke( T1 value1, T2 value2 )
		{
			target.ClassInfo.Functions[function]?.Invoke( target, value1, value2 );
		}
	}
}
