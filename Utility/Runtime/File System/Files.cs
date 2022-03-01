using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Files, is Espionage.Engines File System.
	/// All Saving, Loading, ETC. You can use
	/// short hands for defining paths.
	/// </summary>
	[Library, Group( "Files" ), Title( "File System" )]
	public static class Files
	{
		public static readonly Dictionary<string, string> Paths = new()
		{
			["user"] = Application.persistentDataPath,
			["game"] = Application.dataPath,
			["cache"] = Application.temporaryCachePath,
			["config"] = $"{Application.persistentDataPath}/Data"
		};

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
		/// Load and deserialize the data for us.
		/// </summary>
		public static T Load<T>( string path ) where T : class, IFile
		{
			// Get the actual path
			path = GetPath( path );

			if ( !File.Exists( path ) )
			{
				throw new FileLoadException( "Directory doesn't exist" );
			}

			var library = Library.Database.Get<T>();
			var fileInfo = new FileInfo( path );

			if ( fileInfo.Extension[1..] != library.Components.Get<FileAttribute>()?.Extension )
			{
				Debugging.Log.Info( fileInfo.Extension );
				throw new FileLoadException( "Invalid Extension for File" );
			}

			var file = Library.Database.Create<T>( library.Class );

			file.File = fileInfo;

			using var stream = Read( path );
			file.Load( stream );

			return file;
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
			if ( !path.Contains( "://" ) )
			{
				return path;
			}

			var splitPath = path.Split( "://" );

			// Potential stack overflow if you are stupid
			var grabbedPath = Paths[GetPath( splitPath[0] )];

			return Path.Combine( grabbedPath, splitPath[1] );
		}
	}
}
