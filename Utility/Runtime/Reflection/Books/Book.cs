using System;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	[Serializable]
	public class Book<T> where T : class, ILibrary, new()
	{
		public T Create()
		{
			var classInfo = Library.Database.Get( target );

			using var _ = Debugging.Stopwatch( $"Created Book - {classInfo.Name}" );
			var lib = Library.Database.Create<T>( target );

			// Deserialize Properties
			// If only unity's de / serialization
			// Wasn't ass, I wouldn't have to do this

			for ( var i = 0; i < classInfo.Properties.Length; i++ )
			{
				var key = keys[i];
				var value = values[i];

				var obj = Convert.ChangeType( value, classInfo.Properties[key].Type );
				classInfo.Properties[key][lib] = obj;
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
