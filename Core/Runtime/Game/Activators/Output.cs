using System;
using UnityEngine;

namespace Espionage.Engine.Activators
{
	public struct Output
	{
		public Function Target { get; }
		
		public void Invoke()
		{
			throw new NotImplementedException();
		}
	}
}
