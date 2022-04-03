using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Scheme : Dictionary<string, Binding>
	{
		public Scheme() : base( StringComparer.CurrentCultureIgnoreCase ) { }
	}

	public class Binding
	{
		public KeyCode Key { get; }

		/// <param name="key"> Default Key Bind </param>
		public Binding( KeyCode key )
		{
			Key = key;

			Pressed = false;
			Down = false;
			Released = false;
		}

		public void Sample()
		{
			Pressed = Input.GetKeyDown( Key );
			Down = Input.GetKey( Key );
			Released = Input.GetKeyUp( Key );
		}

		public void Clear()
		{
			Pressed = false;
			Down = false;
			Released = false;
		}

		public bool Down { get; private set; }
		public bool Pressed { get; private set; }
		public bool Released { get; private set; }

		// Helpers

		public static implicit operator Binding( KeyCode code )
		{
			return new( code );
		}
	}
}
