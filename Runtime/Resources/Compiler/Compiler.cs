using System;
using System.Linq;
using Espionage.Engine.IO;
using Espionage.Engine.Resources;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Resources
{
	public static class Compiler
	{
		#if UNITY_EDITOR

		[MenuItem( "Assets/Compile Asset", true )]
		private static bool CompileValidate()
		{
			return Selection.activeObject != null && Exists( Selection.activeObject.GetType() );
		}

		[MenuItem( "Assets/Compile Asset", priority = -500 )]
		private static void Compile()
		{
			foreach ( var selection in Selection.objects )
			{
				Compile( selection, selection.GetType() );
			}
		}

		#endif

		//
		// API
		//

		public static bool Exists( Type type )
		{
			var interfaceType = typeof( ICompiler<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			return library != null;
		}

		public static void Compile<T>( T item )
		{
			var library = Library.Database.Find( typeof( ICompiler<T> ) );
			Assert.IsNull( library );

			Debugging.Log.Info( $"Compiling {item.ToString()}" );
			Library.Create<ICompiler<T>>( library.Info ).Compile( item );
		}

		public static void Compile( object item, Type type )
		{
			var method = typeof( Compiler ).GetMethods().FirstOrDefault( e => e.Name == "Compile" && e.IsGenericMethod )?.MakeGenericMethod( type );
			method?.Invoke( null, new[] { item } );
		}

		public static void Compile( Pathing asset, Type type )
		{
			if ( !asset.Exists() )
			{
				Debugging.Log.Error( $"Path [{asset}] doesn't exist" );
				return;
			}

			var interfaceType = typeof( ICompiler<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			Assert.IsNull( "No Valid Compilers for this Type" );

			var converter = Library.Create( library.Info );
			var method = interfaceType.GetMethod( "Compile" );

			try
			{
				Debugging.Log.Info( $"Compiling {asset.Name()} [{type.Name}]" );
				method?.Invoke( converter, new object[] { asset } );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}
	}
}
