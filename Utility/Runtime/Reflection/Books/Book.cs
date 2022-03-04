using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Book is a Spawnable Reference to a Library.
	/// That is chosen in Editor.
	/// </summary>
	[Serializable]
	public class Book<T> where T : class, ILibrary, new()
	{
		public Dictionary<string, object> Properties { get; private set; }

		public Book() { }

		public Book( string target )
		{
			this.target = target;
		}

		public T Create()
		{
			var classInfo = Library.Database.Get( target );

			using var _ = Debugging.Stopwatch( $"Created Book - {classInfo.Name}" );
			var lib = Library.Database.Create<T>( target );

			// Deserialize Properties
			// If only unity's de / serialization
			// Wasn't ass, I wouldn't have to do this

			if ( Properties == null )
			{
				Properties = new Dictionary<string, object>();

				for ( var i = 0; i < classInfo.Properties.Count; i++ )
				{
					var key = keys[i];
					var value = values[i];

					var obj = Converter.Convert( value, classInfo.Properties[key].Type );
					Properties.Add( key, obj );

					classInfo.Properties[key][lib] = obj;
				}
			}
			else
			{
				for ( var i = 0; i < classInfo.Properties.Count; i++ )
				{
					var key = keys[i];
					classInfo.Properties[key][lib] = Properties[key];
				}
			}

			return lib;
		}

		// Fields

		[SerializeField]
		private string target;

		[SerializeField]
		private string[] keys;

		[SerializeField]
		private string[] values;
	}
}
