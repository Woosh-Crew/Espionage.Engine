using System;
using System.Collections.Generic;
using System.IO;
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
			["cache"] = CachePath,
			["config"] = UserConfigPath,
			["data"] = UserDataPath,

			// -- Editor Specific
			["project"] = "game:///../",
			["exports"] = "project://Exports/",
		};

		private readonly Dictionary<string, string> _keywords = new()
		{
			// -- Game Specific
			["game"] = Application.productName,
			["company"] = Application.companyName
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

		private static string CachePath => GetUserPath( "XDG_CACHE_HOME",
			$"{Environment.GetEnvironmentVariable( "TEMP" )}\\<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.cache/<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Caches/<game>"
		);

		private static string UserDataPath => GetUserPath( "XDG_DATA_HOME",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData )}\\<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.local/share/<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Application Support/<game>"
		);

		private static string UserConfigPath => GetUserPath( "XDG_CONFIG_HOME",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData )}\\<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/.config/<game>",
			$"{Environment.GetFolderPath( Environment.SpecialFolder.UserProfile )}/Library/Preferences/<game>"
		);

		//
		// API
		//

		public void Add( string shortHand, string path )
		{
			if ( _paths.ContainsKey( shortHand ) )
			{
				Debugging.Log.Error( $"Pathing already contains shorthand {shortHand}" );
				return;
			}

			_paths.Add( shortHand, path );
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
		public string Get( string path )
		{
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
			splitPath[0] = Get( _paths[splitPath[0]] );

			var newPath = Path.Combine( splitPath[0], splitPath[1] );

			return Path.GetFullPath( newPath );
		}

		/// <summary>
		/// Gets the path, relative to another path. If you use
		/// the virtual pathing It'll search loaded mods first
		///  then the base content, Depending on the virtual
		/// path you are trying to get.
		/// </summary>
		public string Get( string path, string relative )
		{
			path = Get( path );
			relative = Get( relative );

			return Path.GetRelativePath( relative, path );
		}

		/// <summary>
		/// Will check if the File or Directory exists
		/// </summary>
		public bool Exists( string path )
		{
			path = Get( path );
			return Directory.Exists( path ) || File.Exists( path );
		}
	}
}
