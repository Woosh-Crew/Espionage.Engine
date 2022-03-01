using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Files, is Espionage.Engines File System.
	/// All Saving, Loading, ETC. Path is local to the games
	/// data storage. ( <see cref="Application.dataPath"/> )
	/// </summary>
	[Library, Group( "Input / Output" ), Title( "File System" )]
	public static class Files
	{
		public enum Path
		{
			User,
			Application,
			Cache
		}

		/// <summary>
		/// Just gives us the raw data
		/// from a file at a path
		/// </summary>
		public static byte[] Load( string path, Path directory = Path.Application )
		{
			path = GetPath( path, directory );

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
		public static void Save( string text, string path, Path directory = Path.Application )
		{
			Save( Encoding.UTF8.GetBytes( text ), path );
		}

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public static void Save( byte[] data, string path, Path directory = Path.Application )
		{
			path = GetPath( path, directory );

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
		public static FileStream Read( string path, Path directory = Path.Application )
		{
			path = GetPath( path, directory );
			return new FileStream( path, FileMode.Open, FileAccess.Read );
		}

		public static string GetPath( string path, Path directory )
		{
			return System.IO.Path.Combine( GetPath( directory ), path );
		}

		public static string GetPath( Path path )
		{
			return path switch
			{
				Path.User => Application.persistentDataPath,
				Path.Application => Application.dataPath,
				Path.Cache => Application.temporaryCachePath,
				_ => throw new ArgumentOutOfRangeException( nameof( path ), path, null )
			};
		}
	}
}
