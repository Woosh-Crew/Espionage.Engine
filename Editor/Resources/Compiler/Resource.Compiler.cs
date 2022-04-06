using System;
using UnityEditor;

namespace Espionage.Engine.Resources.Editor
{
	public static class ResourceCompiler
	{
		[MenuItem( "Assets/Compile Asset", true )]
		private static bool CompileValidate()
		{
			return Selection.activeObject != null && Exists( Selection.activeObject.GetType() );
		}

		[MenuItem( "Assets/Compile Asset", priority = -500 )]
		private static void Compile()
		{
			// Find Compiler, and Create it.	
			var selection = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath( selection );

			Compile( path, selection.GetType() );
		}

		private static bool Exists( Type type )
		{
			var interfaceType = typeof( ICompiler<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			return library != null;
		}

		public static void Compile( string asset, Type type )
		{
			if ( !Files.Pathing.Exists( asset ) )
			{
				Dev.Log.Error( $"Path [{asset}] doesn't exist" );
				return;
			}

			var interfaceType = typeof( ICompiler<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			if ( library == null )
			{
				throw new( "No Valid Compilers for this Type" );
			}

			var converter = Library.Database.Create( library.Info );
			var method = interfaceType.GetMethod( "Compile" );

			try
			{
				Dev.Log.Info( $"Compiling {Files.Pathing.Name( asset )} [{type.Name}]" );
				method?.Invoke( converter, new object[] { asset } );
			}
			catch ( Exception e )
			{
				Dev.Log.Exception( e );
			}
		}
	}
}
