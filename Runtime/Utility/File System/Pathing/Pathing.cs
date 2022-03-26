using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine.IO
{
	public class Pathing
	{
		private readonly Dictionary<string, string> _paths = new()
		{
			// -- Game Specific
			["game"] = Application.dataPath,
			["assets"] = Application.isEditor ? "exports://" : "game://",

			// -- User Specific
			["cache"] = GetUserPath( "XDG_CACHE_HOME",
				$"{Environment.GetEnvironmentVariable( "TEMP" )}\\<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.cache/<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Caches/<game>"
			),
			["config"] = GetUserPath( "XDG_CONFIG_HOME",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData )}\\<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.config/<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Preferences/<game>"
			),
			["data"] = GetUserPath( "XDG_DATA_HOME",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData )}\\<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.local/share/<game>",
				$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Application Support/<game>"
			),

			// -- Editor Specific
			["project"] = $"{Application.dataPath}/../",
			["exports"] = "project://Exports/",
			["compiled"] = "exports://<game> <version>/"
		};

		private readonly Dictionary<string, string> _keywords = new()
		{
			// -- Game Specific
			["game"] = Application.productName,
			["version"] = Application.version,
			["company"] = Application.companyName,
			["user"] = Environment.UserName
		};

		//
		// Pathing
		//

		private static string GetUserPath( string xdgVar, string windowsPath, string linuxPath, string macPath )
		{
			var game = Application.productName;

			var xdg = Environment.GetEnvironmentVariable( xdgVar );
			if ( xdg != null )
			{
				return $"{xdg}/{game}";
			}

			return SystemInfo.operatingSystemFamily switch
			{
				OperatingSystemFamily.Windows => windowsPath,
				OperatingSystemFamily.Linux => linuxPath,
				OperatingSystemFamily.MacOSX => macPath,
				OperatingSystemFamily.Other => throw new PlatformNotSupportedException(),
				_ => throw new PlatformNotSupportedException()
			};
		}

		//
		// API
		//

		/// <summary>
		/// Add a shorthand / virtual path to the pathing database.
		/// for use later, you can't override already exising keys.
		/// </summary>
		public void Add( string key, string path )
		{
			if ( _paths.ContainsKey( key ) )
			{
				Dev.Log.Error( $"Pathing already contains shorthand {key}" );
				return;
			}

			_paths.Add( key, path );
		}

		public bool Contains( string item )
		{
			return _paths.ContainsKey( item );
		}

		/// <summary>
		/// Gets the Path, If you use the virtual pathing
		/// It'll search loaded mods first then the base content,
		/// Depending on the virtual path you are trying to get.
		/// </summary>
		public string Absolute( string path )
		{
			Assert.IsNull( path );

			// Change Keywords
			if ( path.Contains( '<' ) )
			{
				foreach ( var (key, value) in _keywords )
				{
					path = path.Replace( $"<{key}>", value );
				}
			}

			// Get Absolute Path
			if ( !path.Contains( "://" ) )
			{
				return path;
			}

			var splitPath = path.Split( "://" );
			splitPath[0] = Absolute( _paths[splitPath[0]] );

			var newPath = Path.Combine( splitPath[0], splitPath[1] );

			return Path.GetFullPath( newPath );
		}

		/// <summary>
		/// Gets the path, relative to another path. If you use
		/// the virtual pathing It'll search loaded mods first
		///  then the base content, Depending on the virtual
		/// path you are trying to get.
		/// </summary>
		public string Relative( string path, string relative )
		{
			path = Absolute( path );
			relative = Absolute( relative );

			return Path.GetRelativePath( relative, path );
		}

		/// <summary>
		/// Will check if the File or Directory exists
		/// </summary>
		public bool Exists( string path )
		{
			path = Absolute( path );
			return Directory.Exists( path ) || File.Exists( path );
		}

		/// <summary>
		/// Gets all files at the given path
		/// </summary>
		public IEnumerable<string> All( string path )
		{
			path = Absolute( path );

			if ( !Exists( path ) )
			{
				return null;
			}

			return Directory.GetFiles( path );
		}

		/// <summary>
		/// <inheritdoc cref="All(string)"/> with an extension
		/// </summary>
		public IEnumerable<string> All( string path, params string[] extension )
		{
			path = Absolute( path );

			if ( !Exists( path ) )
			{
				return null;
			}

			return Directory.GetFiles( path, "*.*", SearchOption.AllDirectories )
				.Where( file => Path.HasExtension( file ) && extension.Contains( Path.GetExtension( file )[1..] ) );
		}


		/// <summary>
		/// Gets the name of last directory or file
		/// at the given path
		/// </summary>
		public string Name( string path, bool withExtension = true )
		{
			path = Absolute( path );
			return withExtension ? Path.GetFileName( path ) : Path.GetFileNameWithoutExtension( path );
		}
	}
}
