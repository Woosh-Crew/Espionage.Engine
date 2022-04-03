using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Scheme : Dictionary<string, Binding>
	{
		public Scheme() : base( StringComparer.CurrentCultureIgnoreCase ) { }
	}

	public readonly struct Binding
	{
		public Binding( KeyCode key )
		{
			Key = key;
		}

		public KeyCode Key { get; }

		public bool Pressed => Input.GetKeyDown( Key );
		public bool Down => Input.GetKey( Key );
		public bool Released => Input.GetKeyUp( Key );
		
		// Helpers
		
		public static implicit operator Binding( KeyCode code )
		{
			return new( code );
		}
	}
}
