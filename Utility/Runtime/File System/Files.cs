using System.IO;
using System.Text;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Files, is Espionage.Engines File System.
	/// All Saving, Loading, ETC, is local to the games
	/// data storage.
	/// </summary>
	[Library, Group( "Input / Output" ), Title( "File System" )]
	public static class Files
	{
		/// <summary>
		/// Just gives us the raw data
		/// from a file at a path
		/// </summary>
		private static byte[] Load( string path )
		{
			path = Path.Combine( Application.dataPath, path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException( "File doesn't Exist" );
			}

			return File.ReadAllBytes( path );
		}

		/// <summary>
		/// Saves a string value to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		private static void Save( string text, string path )
		{
			Save( Encoding.UTF8.GetBytes( text ), path );
		}

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		private static void Save( byte[] data, string path )
		{
			path = Path.Combine( Application.dataPath, path );

			if ( !Directory.Exists( path ) )
			{
				Directory.CreateDirectory( path );
			}

			using var stream = File.Create( path );
			stream.Write( data );
		}

		/// <summary>
		/// Opens a FileStream to the designated path.
		/// </summary>
		private static FileStream Read( string path )
		{
			path = Path.Combine( Application.dataPath, path );

			return new FileStream( path, FileMode.Open, FileAccess.Read );
		}
	}
}
