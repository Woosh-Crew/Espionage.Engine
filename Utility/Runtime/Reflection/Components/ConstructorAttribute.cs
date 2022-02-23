using System;
using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Attribute that allows the definition of a custom constructor.
	/// Must return an ILibrary and Must have one parameter that takes in a Library.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class ConstructorAttribute : Attribute, IConstructor
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
			var method = library.Class.GetMethod( _targetMethod, BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public );

			if ( method is null )
			{
				Debugging.Log.Info( $"Method was not valid {library.Name} - {_targetMethod}" );
				return;
			}

			_library = library;
			_constructor = ( e ) => method.Invoke( null, new object[] { e } );
		}

		// Constructor

		private delegate object Action( Library type );

		private Action _constructor;
		private Library _library;

		public object Invoke()
		{
			return _constructor?.Invoke( _library );
		}
	}
}
