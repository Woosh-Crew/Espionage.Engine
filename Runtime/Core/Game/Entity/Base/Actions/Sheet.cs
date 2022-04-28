using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	[Serializable]
	public struct Sheet
	{
		public string Key => key;
		public string Value => value;

		public static void Apply( IEnumerable<Sheet> template, ILibrary target )
		{
			foreach ( var (key, value) in template )
			{
				var property = target.ClassInfo.Properties[key];

				if ( property is not { Editable: true } )
				{
					return;
				}

				property[target] = property.Type == typeof( string ) ? value : Converter.Convert( value, property.Type );
			}
		}

		public void Deconstruct( out string outKey, out string outValue )
		{
			outKey = Key;
			outValue = Value;
		}

		// Fields

		[SerializeField]
		private string key, value;
	}
}
