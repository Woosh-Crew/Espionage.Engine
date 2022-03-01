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
		/// <summary>
		/// Just gives us the raw data
		/// from a file at a path
		/// </summary>
		public static byte[] Load( string path )
		{
			path = GetPath( path );

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
		public static void Save( string text, string path )
		{
			Save( Encoding.UTF8.GetBytes( text ), path );
		}

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public static void Save( byte[] data, string path )
		{
			path = GetPath( path );

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
		public static FileStream Read( string path )
		{
			path = GetPath( path );
			return new FileStream( path, FileMode.Open, FileAccess.Read );
		}

		private static string GetPath( string path )
		{
			var splittedPath = path.Split( ':' );

			if ( splittedPath.Length < 1 )
			{
				throw new InvalidOperationException( "Invalid Path" );
			}

			var grabbedPath = splittedPath[0] switch
			{
				"user" => Application.persistentDataPath,
				"game" => Application.dataPath,
				"cache" => Application.temporaryCachePath,
				_ => throw new ArgumentOutOfRangeException( nameof( path ), path, null )
			};

			return Path.Combine( grabbedPath, splittedPath[1] );
		}
	}
}
