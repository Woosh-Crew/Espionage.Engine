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
		// BOMless UTF-8 encoder
		private static readonly UTF8Encoding UTF8 = new();

		public static readonly Dictionary<string, string> Paths = new()
		{
			// using our own path methods rather than the dubious unity ones
			["config"] = UserConfigPath(),
			["user"] = UserDataPath(),
			["cache"] = CachePath(),
			["game"] = Application.dataPath
		};


		/// <summary>
		/// Returns a platform-specific cache and temp. data directory path.
		/// </summary>
		private static string CachePath()
		{
			var game = Application.productName;

			// First check if any of the platforms has XDG_CACHE_HOME defined
			var xdg = Environment.GetEnvironmentVariable( "XDG_CACHE_HOME" );
			if ( xdg != null )
			{
				return $"{xdg}/{game}";
			}

			// Otherwise, it's onto the platform specific mess
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			return $"{Environment.GetEnvironmentVariable( "TEMP" )}\\{game}";
		#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.cache/{game}";
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Caches/{game}";
		#else
			throw new PlatformNotSupportedException();
		#endif
		}

		/// <summary>
		/// This method returns the correct platform-specific path for
		/// non-roaming user data files.
		/// </summary>
		/// <remarks>
		/// On Linux at least, Unity's Application.persistentDataPath
		/// dumps all user and config files into $HOME/.config/unity3d.
		/// </remarks>
		private static string UserDataPath()
		{
			var game = Application.productName;

			// First check if any of the platforms has XDG_DATA_HOME defined
			var xdg = Environment.GetEnvironmentVariable( "XDG_DATA_HOME" );
			if ( xdg != null )
			{
				return $"{xdg}/{game}";
			}

			// Otherwise, it's onto the platform specific mess
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			return $"{Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData )}\\{game}";
		#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.local/share/{game}";
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Application Support/{game}";
		#else
			throw new PlatformNotSupportedException();
		#endif
		}

		/// <summary>
		/// Returns the platform-specific directory for user config files.
		/// </summary>
		/// <remarks>
		/// Unity's Application.persistentDataPath uses directories designed
		/// for large user data, rather than config files.
		/// </remarks>
		private static string UserConfigPath()
		{
			var game = Application.productName;

			// First check if any of the platforms has XDG_CONFIG_HOME defined
			var xdg = Environment.GetEnvironmentVariable( "XDG_CONFIG_HOME" );
			if ( xdg != null )
			{
				return $"{xdg}/{game}";
			}

			// Otherwise, it's onto the platform specific mess
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			return $"{Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData )}\\{game}";
		#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config/{game}";
		#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Preferences/{game}";
		#else
			throw new PlatformNotSupportedException();
		#endif
		}

		/// <summary>
		/// Just gives us the raw data
		/// from a file at a path
		/// </summary>
		public static byte[] Load( string path )
		{
			path = GetPath( path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return File.ReadAllBytes( path );
		}

		/// <summary>
		/// Reads a file as one big UTF-8 string.
		/// </summary>
		public static string LoadString( string path )
		{
			path = GetPath( path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return UTF8.GetString( File.ReadAllBytes( path ) );
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
				throw new FileLoadException( "File doesn't exist" );
			}

			var fileInfo = new FileInfo( path );

			var library = Library.Database.Find<T>( e => e.Components.Get<FileAttribute>()?.Extension == fileInfo.Extension[1..] );

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Deserializers for this File" );
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
			Save( UTF8.GetBytes( text ), path );
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

		// TODO: Create a generic Load method for value types such
		// as string, int[], float[], etc. if we can
		// TODO: Generic version of Save for saving arbitrary data other
		// than just string or byte[]

		/// <summary>
		/// Opens a FileStream to the designated path.
		/// </summary>
		public static FileStream Read( string path )
		{
			path = GetPath( path );
			return new FileStream( path, FileMode.Open, FileAccess.Read );
		}

		/// <summary>
		/// Gets the Path, If you use the virtual pathing
		/// It'll search loaded mods first then the base content,
		/// Depending on the virtual path you are trying to get.
		/// </summary>
		public static string GetPath( string path )
		{
			if ( !path.Contains( "://" ) )
			{
				return path;
			}

			var splitPath = path.Split( "://" );
			splitPath[0] = GetPath( Paths[splitPath[0]] );

			return Path.Combine( splitPath[0], splitPath[1] );
		}
	}
}
