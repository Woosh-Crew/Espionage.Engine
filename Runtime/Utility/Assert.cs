using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Espionage.Engine
{
	/// <summary>
	/// This class is a helper class for throwing exceptions
	/// when an operation is invalid.
	/// </summary>
	public static class Assert
	{
		[Conditional( "DEBUG" )]
		public static void IsEmpty( ICollection collection, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( collection.Count == 0 )
			{
				Fail( $"Collection was Empty! [{caller}]" );
			}
		}

		[Conditional( "DEBUG" )]
		public static void IsNull<T>( T item, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( item == null )
			{
				Fail( $"Item was NULL! [{caller}]" );
			}
		}

		// Bool

		[Conditional( "DEBUG" )]
		public static void IsEqual<T>( T item, T comparison, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( item.Equals( comparison ) )
			{
				Fail( $"{typeof( T ).Name} was equals comparison! [{caller}]" );
			}
		}

		[Conditional( "DEBUG" )]
		public static void IsTrue( bool item, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( item )
			{
				Fail( $"Bool was True! [{caller}]" );
			}
		}

		[Conditional( "DEBUG" )]
		public static void IsFalse( bool item, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( item == false )
			{
				Fail( $"Bool was False! [{caller}]" );
			}
		}

		// Utility

		private static void Fail( string message )
		{
			throw new( message );
		}
	}
}
