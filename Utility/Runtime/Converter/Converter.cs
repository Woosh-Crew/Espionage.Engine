using System;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Espionage.Engine
{
	/// <summary>
	/// Espionage.Engine's string to object converter.
	/// We sometimes use this for deserializing Properties.
	/// </summary>
	public static class Converter
	{
	#if UNITY_EDITOR

		[MenuItem( "Tools/Espionage.Engine/Debug/Report Converter" )]
		private static void Test()
		{
			var value = Random.Range( 0, 10 ).ToString();

			using ( Debugging.Stopwatch( "Converting Int" ) )
			{
				var converted = Convert<int>( value );
				Debugging.Log.Info( $"Converted Value = {converted}, original string {value}" );
			}

			value = "accept";

			using ( Debugging.Stopwatch( "Converting Bool" ) )
			{
				var converted = Convert<bool>( value );
				Debugging.Log.Info( $"Converted Value = {converted}, original string {value}" );
			}

			value = new Vector3( 5, 8, 15 ).ToString();

			using ( Debugging.Stopwatch( "Converting Vector3" ) )
			{
				var converted = Convert<Vector3>( value );
				Debugging.Log.Info( $"Converted Value = {converted}, original string {value}" );
			}
		}

	#endif


		/// <summary>
		/// Converts a string to the type of T. 
		/// </summary>
		public static T Convert<T>( string value )
		{
			var library = Library.Database.Find<IConverter<T>>();

			if ( library == null )
			{
				throw new Exception( "No Valid Converters for this Type" );
			}

			var converter = Library.Database.Create<IConverter<T>>( library.Class );

			try
			{
				return converter.Convert( value );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
				return default;
			}
		}

		/// <summary>
		/// Converts a string to an object. Based off the
		/// inputted type. 
		/// </summary>
		/// <remarks>
		/// This uses reflection, and is pretty slow..
		/// Be careful where you put this method.
		/// </remarks>
		public static object Convert( string value, Type type )
		{
			// JAKE: This is so aids.... But can't do much about that.

			var interfaceType = typeof( IConverter<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			if ( library == null )
			{
				throw new Exception( "No Valid Converters for this Type" );
			}

			var converter = Library.Database.Create( library.Class );
			var method = interfaceType.GetMethod( "Convert" );

			try
			{
				return method?.Invoke( converter, new object[] { value } );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
				return default;
			}
		}
	}
}
