using System;
using Espionage.Engine.Resources;
using UnityEditor;

namespace Espionage.Engine.Editor.Resources
{
	public static class Tester
	{
		public static string Application { get; set; } = "compiled://<executable>";

		//
		// Editor Menu Items
		//

		[MenuItem( "Tools/Espionage.Engine/Testing Target/Exports" )]
		private static void ToggleAction()
		{
			Application = "compiled://<executable>";
		}

		[MenuItem( "Tools/Espionage.Engine/Testing Target/Exports", true )]
		private static bool ToggleActionValidate()
		{
			UnityEditor.Menu.SetChecked( "Tools/Espionage.Engine/Testing Target/Exports", Application == "compiled://<executable>" );
			return true;
		}


		[MenuItem( "Assets/Test Asset", true )]
		private static bool TestValidate()
		{
			if ( !Files.Pathing( "compiled://" ).Exists() )
			{
				return false;
			}

			if ( Selection.objects.Length > 1 )
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

			Test( path, selection.GetType() );
		}

		[MenuItem( "Assets/Compile and Test Asset", true )]
		private static bool BothValidate()
		{
			return TestValidate() && Compiler.Exists( Selection.activeObject?.GetType() );
		}

		[MenuItem( "Assets/Compile and Test Asset", priority = -600 )]
		private static void Both()
		{
			// Find Compiler, and Create it.	
			var selection = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath( selection );

			Compiler.Compile( selection, selection.GetType() );
			Test( path, selection.GetType() );
		}

		//
		// API
		//

		public static bool Exists( Type type )
		{
			var interfaceType = typeof( ITester<> ).MakeGenericType( type );
			var library = Library.Database.Find( interfaceType );

			return library != null;
		}

		public static void Test( string asset, Type type )
		{
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
				Messages.Send( (string)launchArgs, Application );
				Debugging.Log.Info( (string)Files.Pathing( Application ).Absolute() );
			}
			catch ( Exception e )
			{
				UnityEngine.Debug.LogException( e );
			}
		}
	}
}
