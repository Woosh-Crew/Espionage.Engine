using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
#endif

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
		internal static readonly Dictionary<string, string> Paths = new()
		{
			// using our own path methods rather than the dubious unity ones
			["config"] = UserConfigPath,
			["user"] = UserDataPath,
			["cache"] = CachePath,
			["game"] = Application.dataPath,
			["assets"] = Application.isEditor ? $"exports://" : Application.dataPath,

	#if UNITY_EDITOR
			["exports"] = $"{Application.dataPath}/../Exports/",
			["project"] = $"{Application.dataPath}/../",
			["package"] = PackagePath,
	#endif

		};

	#if UNITY_EDITOR

		[MenuItem( "Tools/Espionage.Engine/Debug/Files/Report Random File" )]
		private static void Testing()
		{
			Save( "Testing", "game://test.txt" );
			Debugging.Log.Info( Deserialize<string>( "game://test.txt" ) );
			Delete( "game://test.txt" );
		}

		[MenuItem( "Tools/Espionage.Engine/Debug/Files/Debug All Virtual Locations" )]
		private static void Testing2()
		{
			foreach ( var (key, value) in Paths )
			{
				Debugging.Log.Info( $"{key} - [{Path( value )}]" );
			}
		}

		[MenuItem( "Tools/Espionage.Engine/Debug/Files/Open Cookies" )]
		private static void Testing3()
		{
			Open( "config://.cookies" );
		}

	#endif

		//
		// Pathing
		//

		/// <summary>
		/// Returns a platform-specific cache and temp. data directory path.
		/// </summary>
		private static string CachePath
		{
			get
			{
				var game = Application.productName;

				// First check if any of the platforms has XDG_CACHE_HOME defined
				var xdg = Environment.GetEnvironmentVariable( "XDG_CACHE_HOME" );
				if ( xdg != null )
				{
					return $"{xdg}/{game}";
				}

				return SystemInfo.operatingSystemFamily switch
				{
					OperatingSystemFamily.Windows => $"{Environment.GetEnvironmentVariable( "TEMP" )}\\{game}",
					OperatingSystemFamily.Linux => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.cache/{game}",
					OperatingSystemFamily.MacOSX => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Caches/{game}",
					OperatingSystemFamily.Other => throw new PlatformNotSupportedException(),
					_ => throw new PlatformNotSupportedException()
				};
			}
		}

		/// <summary>
		/// This method returns the correct platform-specific path for
		/// non-roaming user data files.
		/// </summary>
		/// <remarks>
		/// On Linux at least, Unity's Application.persistentDataPath
		/// dumps all user and config files into $HOME/.config/unity3d.
		/// </remarks>
		private static string UserDataPath
		{
			get
			{
				var game = Application.productName;

				// First check if any of the platforms has XDG_DATA_HOME defined
				var xdg = Environment.GetEnvironmentVariable( "XDG_DATA_HOME" );
				if ( xdg != null )
				{
					return $"{xdg}/{game}";
				}

				return SystemInfo.operatingSystemFamily switch
				{
					OperatingSystemFamily.MacOSX => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Application Support/{game}",
					OperatingSystemFamily.Windows => $"{Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData )}\\{game}",
					OperatingSystemFamily.Linux => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.local/share/{game}",
					OperatingSystemFamily.Other => throw new PlatformNotSupportedException(),
					_ => throw new PlatformNotSupportedException()
				};
			}
		}

		/// <summary>
		/// Returns the platform-specific directory for user config files.
		/// </summary>
		/// <remarks>
		/// Unity's Application.persistentDataPath uses directories designed
		/// for large user data, rather than config files.
		/// </remarks>
		private static string UserConfigPath
		{
			get
			{
				var game = Application.productName;

				// First check if any of the platforms has XDG_CONFIG_HOME defined
				var xdg = Environment.GetEnvironmentVariable( "XDG_CONFIG_HOME" );
				if ( xdg != null )
				{
					return $"{xdg}/{game}";
				}

				return SystemInfo.operatingSystemFamily switch
				{
					OperatingSystemFamily.MacOSX => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Preferences/{game}",
					OperatingSystemFamily.Windows => $"{Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData )}\\{game}",
					OperatingSystemFamily.Linux => $"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.config/{game}",
					OperatingSystemFamily.Other => throw new PlatformNotSupportedException(),
					_ => throw new PlatformNotSupportedException()
				};
			}
		}

		private static string PackagePath
		{
			get
			{
				var package = PackageInfo.FindForAssembly( Assembly.GetExecutingAssembly() );
				return package == null ? "game://Espionage.Engine/" : $"Packages/{package.name}/";
			}
		}

		//
		// Deserializing
		//

		/// <summary>
		/// Load and deserialize the data for us. Will try and find
		/// the IFile that contains the respective extension.
		/// </summary>
		public static T Load<T>( string path ) where T : class, IFile
		{
			// Get the actual path
			path = Path( path );

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
		/// Deserializes data at the given path. Will
		/// automatically deserialize it to the target
		/// format.
		/// </summary>
		public static T Deserialize<T>( string path )
		{
			path = Path( path );

			var deserializer = GrabDeserializer<T>();
			return deserializer.Deserialize( Deserialize( path ) );
		}

		/// <summary>
		/// Just gives us the raw data from a file at a path
		/// </summary>
		public static byte[] Deserialize( string path )
		{
			path = Path( path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return File.ReadAllBytes( path );
		}

		private static IDeserializer<T> GrabDeserializer<T>()
		{
			var library = Library.Database.Find<IDeserializer<T>>();

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Deserializers for this File" );
			}

			return Library.Database.Create<IDeserializer<T>>( library.Class );
		}

		//
		// Serialization
		//

		/// <summary>
		/// Saves anything you want, (provided theres a
		/// serializer for it) to the given path
		/// </summary>
		public static void Save<T>( T item, string path )
		{
			Save( Serialize( item ), path );
		}

		/// <summary>
		/// Saves an array of anything you want,
		/// (provided theres a serializer for it)
		/// to the given path
		/// </summary>
		public static void Save<T>( T[] item, string path )
		{
			Save( Serialize( item ), path );
		}

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public static void Save( byte[] data, string path )
		{
			path = Path( path );

			var fileInfo = new FileInfo( path );

			if ( !Directory.Exists( fileInfo.DirectoryName ) )
			{
				Directory.CreateDirectory( fileInfo.DirectoryName );
			}

			using var stream = File.Create( path );
			stream.Write( data );
		}

		/// <summary>
		/// Serialize type of T to a byte array.
		/// </summary>
		public static byte[] Serialize<T>( T data )
		{
			var serializer = GrabSerializer<T>();
			return serializer.Serialize( data );
		}

		/// <summary>
		/// Serialize an type array of T to a byte array.
		/// </summary>
		public static byte[] Serialize<T>( T[] data )
		{
			var serializer = GrabSerializer<T>();
			return serializer.Serialize( data );
		}

		private static ISerializer<T> GrabSerializer<T>()
		{
			var library = Library.Database.Find<ISerializer<T>>();

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Serializers for this File" );
			}

			return Library.Database.Create<ISerializer<T>>( library.Class );
		}

		//
		// Reading
		//

		/// <summary>
		/// Opens a FileStream to the designated path.
		/// </summary>
		public static FileStream Read( string path )
		{
			path = Path( path );
			return new( path, FileMode.Open, FileAccess.Read );
		}

		/// <summary>
		/// Gets the Path, If you use the virtual pathing
		/// It'll search loaded mods first then the base content,
		/// Depending on the virtual path you are trying to get.
		/// </summary>
		public static string Path( string path )
		{
			if ( !path.Contains( "://" ) )
			{
				return path;
			}

			var splitPath = path.Split( "://" );
			splitPath[0] = Path( Paths[splitPath[0]] );

			var newPath = System.IO.Path.Combine( splitPath[0], splitPath[1] );

			return System.IO.Path.GetFullPath( newPath );
		}

		public static string Path( string path, string relative )
		{
			path = Path( path );
			relative = Path( relative );

			return System.IO.Path.GetRelativePath( relative, path );
		}

		//
		// Utility
		//

		/// <summary>
		/// Will check if the File or Directory exists
		/// </summary>
		public static bool Exists( string path )
		{
			path = Path( path );
			return System.IO.Path.HasExtension( path ) ? File.Exists( path ) : Directory.Exists( path );
		}

		/// <summary>
		/// Deletes the file at the given path
		/// </summary>
		public static void Delete( string path )
		{
			path = Path( path );

			if ( File.Exists( path ) )
			{
				File.Delete( path );
			}

			Debugging.Log.Error( $"File [{path}], couldn't be deleted." );
		}

		/// <summary>
		/// Deletes all files with the given extension at the path
		/// </summary>
		public static void Delete( string path, string extension )
		{
			path = Path( path );

			var files = Directory.GetFiles( path, $"*.{extension}" );
			foreach ( var item in files )
			{
				File.Delete( item );
			}
		}

		/// <inheritdoc cref="Delete(string, string)"/> 
		public static void Delete( string path, params string[] extension )
		{
			if ( !Exists( path ) )
			{
				Debugging.Log.Error( $"Path [{path}], doesn't exist" );
				return;
			}

			foreach ( var item in extension )
			{
				Delete( path, item );
			}
		}


		/// <summary>
		/// Copies the source file to the target path
		/// </summary>
		public static void Copy( string file, string path )
		{
			file = Path( file );
			path = Path( path );

			var fileInfo = new FileInfo( file );

			if ( !File.Exists( file ) )
			{
				throw new FileNotFoundException();
			}

			if ( !Directory.Exists( path ) )
			{
				Directory.CreateDirectory( path );
			}

			fileInfo.CopyTo( path );
		}

		/// <summary>
		/// Moves the source file to the target destination
		/// </summary>
		public static void Move( string source, string destination, bool overwrite = true )
		{
			source = Path( source );
			destination = Path( destination );

			if ( !File.Exists( source ) )
			{
				throw new FileNotFoundException();
			}

			File.Move( source, destination );
		}

		/// <summary>
		/// Opens the given directory in the OS's File Explorer,
		/// or opens the given file in the default application
		/// </summary>
		public static void Open( string path )
		{
			path = Path( path );

			if ( !Exists( path ) )
			{
				Debugging.Log.Warning( $"Path or File [{path}], doesn't exist" );
			}

			Process.Start( $"file://{path}" );
		}
	}
}
