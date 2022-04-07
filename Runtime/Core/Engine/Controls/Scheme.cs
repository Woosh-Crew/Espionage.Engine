using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Scheme : IEnumerable<Binding>
	{
		private readonly Dictionary<string, Binding> _storage = new( StringComparer.CurrentCultureIgnoreCase );
		private readonly Binding _default = new( null );

		public Binding this[ string key ]
		{
			get
			{
				if ( _storage.ContainsKey( key ) )
				{
					return _storage[key];
				}

				Dev.Log.Error( $"Controls Scheme doesn't have [{key}] Binding" );
				return _default;
			}
		}

		public void Add( string key, KeyCode item )
		{
			Add( key, new Binding( item ) );
		}

		public void Add( string key, Binding bind )
		{
			if ( _storage.ContainsKey( key ) )
			{
				Dev.Log.Warning( $"Replacing Binding {key}" );
				_storage[key] = bind;
				return;
			}

			_storage.Add( key, bind );
		}

		// Enumerator

		public IEnumerator<Binding> GetEnumerator()
		{
			return _storage.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class Binding
	{
		public KeyCode? Key { get; private set; }
		public string Text { get; private set; }

		public Binding( KeyCode? key )
		{
			Change( key );
		}

		public override string ToString()
		{
			return Text;
		}

		public void Change( KeyCode? key )
		{
			Key = key;
			Text = key?.ToString() ?? "None";
		}

		public void Sample()
		{
			Pressed = Key.HasValue && Input.GetKeyDown( Key.Value );
			Down = Key.HasValue && Input.GetKey( Key.Value );
			Released = Key.HasValue && Input.GetKeyUp( Key.Value );
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
	}
}
