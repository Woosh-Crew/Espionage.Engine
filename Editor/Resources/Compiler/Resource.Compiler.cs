using System;
using UnityEditor;

namespace Espionage.Engine.Resources.Editor
{
	public static class ResourceCompiler
	{
		[MenuItem( "Assets/Compile Selected Asset", true )]
		private static bool CompileValidate()
		{
			return Selection.activeObject != null && Exists( Selection.activeObject.GetType() );

		}


		[MenuItem( "Assets/Compile Selected Asset", priority = -500 )]
		private static void Compile()
		{
			// Find Compiler, and Create it.	
			var selection = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath( selection );

			Grab( path, selection.GetType() );
		}

		private static bool Exists( Type type )
		{
			var interfaceType = typeof( ICompiler<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			return library != null;
		}

		private static void Grab( string asset, Type type )
		{
			// JAKE: This is so aids.... But can't do much about that.

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
				method?.Invoke( converter, new object[] { asset } );
			}
			catch ( Exception e )
			{
				Dev.Log.Exception( e );
			}
		}
	}
}
