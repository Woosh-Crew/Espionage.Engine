using System.Collections;
using System.Runtime.CompilerServices;

namespace Espionage.Engine
{
	/// <summary>
	/// This class is a helper class for throwing exceptions
	/// when an operation is invalid.
	/// </summary>
	public static class Assert
	{
		public static void IsEmpty( ICollection collection, [CallerMemberName] string caller = null )
		{
			if ( collection.Count == 0 )
				Fail( $"Collection was Empty! {caller}" );
		}
		
		public static void IsNull<T>( T item, [CallerMemberName] string caller = null )
		{
			if ( item == null )
				Fail( $"Item was NULL! {caller}" );
		}
		
		// Bool

		public static void IsTrue( bool item, [CallerMemberName] string caller = null )
		{
			if ( item )
				Fail( $"Bool was True! {caller}" );
		}

		public static void IsFalse( bool item, [CallerMemberName] string caller = null )
		{
			if ( item == false )
				Fail( $"Bool was False! {caller}" );
		}
		
		// Utility

		private static void Fail( string message )
		{
			throw new( message );
		}
	}
}
