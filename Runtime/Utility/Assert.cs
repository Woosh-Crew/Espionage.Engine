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
		public static void IsEmpty( ICollection collection, string message = "Collection was Empty!", [CallerLineNumber] int caller = default )
		{
			if ( collection.Count == 0 )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		public static void IsNull<T>( T item, string message = "Item was NULL!", [CallerLineNumber] int caller = default )
		{
			if ( item == null )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		public static void IsNotNull<T>( T item, string message = "Item was not NULL!", [CallerLineNumber] int caller = default )
		{
			if ( item != null )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		// Bool

		public static void IsEqual<T>( T item, T comparison, string message = null, [CallerLineNumber] int caller = default )
		{
			if ( item.Equals( comparison ) )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		public static void IsTrue( bool item, string message = "Bool was True!", [CallerLineNumber] int caller = default )
		{
			if ( item )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		public static void IsFalse( bool item, string message = "Bool was False!", [CallerLineNumber] int caller = default )
		{
			if ( item == false )
			{
				Fail( $"{message} [{caller}]" );
			}
		}

		// Utility

		private static void Fail( string message )
		{
			throw new( message );
		}
	}
}
