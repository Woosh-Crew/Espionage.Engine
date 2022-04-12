﻿using System;
using System.Diagnostics;
using UnityEditor;

namespace Espionage.Engine.Resources.Editor
{
	public static class ResourceTester
	{
		[MenuItem( "Assets/Test Asset", true )]
		private static bool TestValidate()
		{
			if ( !Files.Pathing.Exists( "compiled://" ) )
			{
				return false;
			}

			return Selection.activeObject != null && Exists( Selection.activeObject.GetType() );
		}

		[MenuItem( "Assets/Test Asset", priority = -500 )]
		private static void Test()
		{
			// Find Compiler, and Create it.	
			var selection = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath( selection );

			Grab( path, selection.GetType() );
		}

		[MenuItem( "Assets/Compile and Test Asset", priority = -600 )]
		private static void Both()
		{
			// Find Compiler, and Create it.	
			var selection = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath( selection );

			ResourceCompiler.Compile( path, selection.GetType() );
			Grab( path, selection.GetType() );
		}

		private static bool Exists( Type type )
		{
			var interfaceType = typeof( ITester<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			return library != null;
		}

		private static void Grab( string asset, Type type )
		{
			// JAKE: This is so aids.... But can't do much about that.

			var interfaceType = typeof( ITester<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			if ( library == null )
			{
				throw new( "No Valid Compilers for this Type" );
			}

			var converter = Library.Create( library.Info );
			var method = interfaceType.GetMethod( "Test" );

			try
			{
				var launchArgs = method?.Invoke( converter, new object[] { asset } );
				Messages.Send( (string)launchArgs );
			}
			catch ( Exception e )
			{
				UnityEngine.Debug.LogException( e );
			}
		}
	}
}
