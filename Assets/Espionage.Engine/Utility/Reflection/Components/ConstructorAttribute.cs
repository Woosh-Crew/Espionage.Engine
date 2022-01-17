using System;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary> Attribute that allows the definition of a custom constructor
	/// Must return an ILibrary and Must have one parameter that takes in a Library </summary>
	public sealed class ConstructorAttribute : Attribute, Library.IComponent
	{
		// Attribute

		private readonly string _targetMethod;

		/// <param name="constructor"> Method should return ILibrary </param>
		public ConstructorAttribute( string constructor )
		{
			_targetMethod = constructor;
		}

		// Component

		public void OnAttached( ref Library library )
		{
			var method = library.Class.GetMethod( _targetMethod, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );

			if ( method is null )
			{
				return;
			}

			_constructor = ( e ) => method.Invoke( null, new object[] { e } ) as ILibrary;
			_library = library;
		}

		// Constructor

		private delegate ILibrary Action( Library type );

		private Action _constructor;
		private Library _library;

		public ILibrary Invoke()
		{
			return _constructor( _library );
		}
	}
}
