using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	public partial class Library
	{
		//
		// Public API
		//

		/// <summary> Database for library records </summary>
		public static IDatabase<Library> Database => _database;

		/// <summary> Attribute that skips the attached class from generating a library reference </summary>
		[AttributeUsage( AttributeTargets.Class )]
		public class Skip : Attribute { }


		/// <summary> Attribute that allows the definition of a custom constructor </summary>
		[AttributeUsage( AttributeTargets.Class, Inherited = true )]
		public sealed class Constructor : Attribute
		{
			/// <param name="constructor"> Method should return ILibrary </param>
			public Constructor( string constructor )
			{
				this.constructor = constructor;
			}

			private string constructor;
			public string Target => constructor;
		}
	}
}
