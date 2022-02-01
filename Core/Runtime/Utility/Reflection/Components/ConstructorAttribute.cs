using System;
using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary> Attribute that allows the definition of a custom constructor
	/// Must return an ILibrary and Must have one parameter that takes in a Library </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class ConstructorAttribute : Attribute, IComponent<Library>
	{
		// Attribute

		private readonly string _targetMethod;

		/// <param name="constructor"> Method should return ILibrary </param>
		public ConstructorAttribute( string constructor )
		{
			_targetMethod = constructor;
		}

		// Component

		public void OnAttached( Library library )
		{
			var method = library.Class.GetMethod( _targetMethod, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

			if ( method is null )
			{
				return;
			}

			_constructor = ( e ) => method.Invoke( null, new object[] { e } );
			_library = library;
		}

		// Constructor

		private delegate object Action( Library type );

		private Action _constructor;
		private Library _library;

		public object Invoke()
		{
			return _constructor( _library );
		}
	}
}
