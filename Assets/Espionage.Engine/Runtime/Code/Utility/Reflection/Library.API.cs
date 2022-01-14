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
		/// <summary> Database for library records </summary>
		public static IDatabase<Library> Database => _database;

		/// <summary> Attribute that skips the attached class from generating a library reference </summary>
		[AttributeUsage( AttributeTargets.Class, Inherited = true )]
		public class Skip : Attribute { }

		//
		// Components
		//

		public interface IComponent
		{
			Library Library { get; set; }

			void OnAttached() { }
			void OnDetached() { }
		}

		/// <summary> Attribute that allows the definition of a custom constructor
		/// Must return an ILibrary and Must have one parameter that takes in a Library </summary>
		public sealed class Constructor : Attribute, IComponent
		{
			// Attribute

			private string targetMethod;

			/// <param name="constructor"> Method should return ILibrary </param>
			public Constructor( string constructor )
			{
				this.targetMethod = constructor;
			}

			// Component

			public Library Library { get; set; }

			public void OnAttached()
			{
				// Works?
				_constructor = GetConstructor();
			}

			// Constructor

			private delegate ILibrary Action( Library type );
			private Action _constructor;


			private Action GetConstructor()
			{
				var method = Library.Class.GetMethod( targetMethod, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

				if ( method is null )
					return null;

				return ( e ) => method.Invoke( null, new object[] { e } ) as ILibrary;
			}

			public ILibrary Invoke()
			{
				return _constructor( Library );
			}

		}
	}
}
