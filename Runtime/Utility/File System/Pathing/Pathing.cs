using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.IO
{
	public struct Pathing
	{
		private readonly struct Grouping
		{
			public Grouping( string path, bool overridable )
			{
				Path = path;
				Overridable = overridable;
			}

			public static implicit operator Grouping( string value )
			{
				return new( value, false );
			}

			public string Path { get; }
			public bool Overridable { get; }
		}

		private static readonly Dictionary<string, Grouping> Paths = new( StringComparer.OrdinalIgnoreCase )
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

			#if UNITY_EDITOR

			// -- Editor Specific
			["project"] = $"{Application.dataPath}/../",
			["exports"] = "project://Exports/",
			["compiled"] = "exports://<game>/",
			["editor"] = EditorApplication.applicationPath,

			#endif
		};

		// Reason why its a func, is cause some of these values are null
		// when the static constructor gets called, so we use the func instead
		private static readonly Dictionary<string, Func<string[], string>> Keywords = new()
		{
			// -- Game Specific
			["game"] = ( _ ) => Engine.Game?.ClassInfo.Title ?? "None",
			["executable"] = ( _ ) => Engine.Game == null ? "error" : $"{Engine.Game.ClassInfo.Name}.exe",
			["company"] = ( _ ) => Application.companyName,
			["user"] = ( _ ) => Environment.UserName,

			// Library
			["library_title"] = ( args ) => Library.Database[args[0]]?.Title,
			["library_group"] = ( args ) => Library.Database[args[0]]?.Group
		};

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

		/// <summary>
		/// Add a shorthand / virtual path to the pathing database
		/// for use later, you can't override already exising keys.
		/// </summary>
		public static void Add( string key, string path, bool overrideable = false )
		{
			if ( path.IsEmpty() )
			{
				Debugging.Log.Warning( $"Path [{path}] for key [{key}] was empty / null!" );
			}

			if ( Paths.ContainsKey( key ) )
			{
				Debugging.Log.Error( $"Pathing already contains shorthand {key}" );
				return;
			}

			Paths.Add( key, new( path, overrideable ) );
		}

		/// <summary>
		/// Add a keyword to the pathing database for use later, 
		/// you can't override already exising keys.
		/// The string[] are optional parameters you can include
		/// into the Keyword, they are divided by a comma.
		/// </summary>
		public static void Add( string key, Func<string[], string> word )
		{
			if ( Keywords.ContainsKey( key ) )
			{
				Debugging.Log.Error( $"Pathing already contains keyword {key}" );
				return;
			}

			Keywords.Add( key, word );
		}

		/// <summary>
		/// Does the paths database contain this key?
		/// </summary>
		public static bool Contains( string path )
		{
			return Paths.ContainsKey( path );
		}

		/// <summary>
		/// Can this path be overriden by mods? (such as "models://")
		/// </summary>
		/// <returns> True if I can be overriden </returns>
		public static bool IsOverridable( string path )
		{
			return Paths[path].Overridable;
		}

		//
		// Pathing
		//

		public string Output { get; private set; }

		internal Pathing( string path )
		{
			Output = path;
		}

		public override string ToString()
		{
			return Output;
		}

		// Operators

		public static implicit operator string( Pathing pathing )
		{
			return pathing.Output;
		}

		public static implicit operator Pathing( string pathing )
		{
			return new( pathing );
		}

		public static Pathing operator +( Pathing left, Pathing b )
		{
			left.Output += b.Output;
			return left;
		}

		public static Pathing operator +( Pathing left, string b )
		{
			left.Output += b;
			return left;
		}

		//
		// Builders
		//

		/// <summary>
		/// Gets the Path, If you use the virtual pathing
		/// It'll search loaded mods first then the base content,
		/// Depending on the virtual path you are trying to get.
		/// </summary>
		public Pathing Absolute( bool directoryOnly = false )
		{
			var path = Output;

			Assert.IsNull( path );

			// Change Keywords
			if ( path.Contains( '<' ) )
			{
				foreach ( var (key, value) in Keywords )
				{
					if ( !path.Contains( $"<{key}" ) )
					{
						continue;
					}

					var concat = path.Split( '<', '>' )[1];

					// Get parameters
					if ( concat.Contains( '(' ) )
					{
						var args = concat.Split( '(', ')' )[1];
						path = path.Replace( $"<{concat}>", value.Invoke( args.Split( ',' ) ) );
						continue;
					}

					// No Args
					path = path.Replace( $"<{key}>", value.Invoke( null ) );
				}
			}

			// Get Absolute Path
			if ( !path.Contains( "://" ) )
			{
				path = Path.GetFullPath( path );

				Output = directoryOnly ? Path.GetDirectoryName( path ) : path;
				return this;
			}

			var splitPath = path.Split( "://" );
			var virtualPath = splitPath[0];

			if ( !Paths.ContainsKey( virtualPath ) )
			{
				throw new( $"Path {virtualPath} isn't present in the Keys. Make sure you have valid pathing." );
			}

			splitPath[0] = new Pathing( Paths[virtualPath].Path ).Absolute();

			var newPath = Path.GetFullPath( Path.Combine( splitPath[0], splitPath[1] ) );

			// See if we can override it.
			if ( !IsOverridable( virtualPath ) )
			{
				Output = directoryOnly ? Path.GetDirectoryName( newPath ) : newPath;
				return this;
			}

			// Go through mods, see if we can replace it.
			// foreach ( var mod in Mod.Database )
			// {
			// 	if ( mod.Exists( newPath, out var potentialPath ) )
			// 	{
			// 		return directoryOnly ? Path.GetDirectoryName( potentialPath ) : potentialPath;
			// 	}
			// }

			Output = directoryOnly ? Path.GetDirectoryName( newPath ) : newPath;
			return this;
		}

		/// <summary>
		/// Creates a directory at the given path
		/// if the directory didnt exist.
		/// </summary>
		public Pathing Create()
		{
			if ( !Exists() )
			{
				Directory.CreateDirectory( Output );
			}

			return this;
		}

		/// <summary>
		/// Gets the path, relative to another path. If you use
		/// the virtual pathing It'll search loaded mods first
		/// then the base content, Depending on the virtual
		/// path you are trying to get.
		/// </summary>
		public Pathing Relative( Pathing relative )
		{
			Output = Path.GetRelativePath( relative.Absolute(), Files.Pathing( Output ).Absolute() );
			return this;
		}

		/// <summary>
		/// Converts this pathing to be virtual path. Useful for loading over
		/// the network, where files would be relative to a shorthand
		/// </summary>
		public Pathing Virtual()
		{
			var potential = string.Empty;
			var shorthand = string.Empty;

			foreach ( var (key, value) in Paths )
			{
				var pathing = Files.Pathing( value.Path ).Absolute();

				if ( pathing.Meta().IsFile )
				{
					continue;
				}

				var relative = Files.Pathing( Output ).Relative( pathing );

				// Invalid Pathing
				if ( !relative.IsRelative() || relative.Output.StartsWith( @"..\" ) )
				{
					continue;
				}

				// Has shortest relative path length, must be the right shorthand 
				if ( relative.Output.Length < potential.Length || potential.IsEmpty() )
				{
					potential = relative;
					shorthand = key;
				}
			}

			if ( potential.IsEmpty() )
			{
				Debugging.Log.Warning( $"Couldn't make [{Output}] virtual" );
				return this;
			}

			Output = $"{shorthand}://{potential}";
			return this;
		}

		//
		// Outputs
		//

		/// <summary>
		/// Gets the files meta at the given path.
		/// Meta includes its attributes, when it was
		/// created, the lsat access time and the last
		/// write time.
		/// </summary>
		public Files.Meta Meta()
		{
			var path = Files.Pathing( Output ).Absolute();
			return !path.Exists()
				? default
				: new Files.Meta(
					File.GetAttributes( path ),
					File.GetCreationTime( path ),
					File.GetLastAccessTime( path ),
					File.GetLastWriteTime( path )
				);
		}

		/// <summary>
		/// Checks if this path is a valid path. Meaning it'll check
		/// if it is a string that could potentially lead to a path. 
		/// </summary>
		public bool IsValid()
		{
			// It works... Don't complain.
			return Path.IsPathFullyQualified( Output ) || Output.Contains( "://" );
		}

		/// <summary>
		/// Checks if a path is a virtual path (Meaning it has a shorthand assigner)
		/// </summary>
		public bool IsVirtual()
		{
			return Output.Contains( "://" ) && Paths.ContainsKey( Output.Split( "://" )[0] );
		}

		/// <summary>
		/// Checks if the path is empty or null 
		/// </summary>
		public bool IsEmpty()
		{
			return Output.IsEmpty();
		}

		/// <summary>
		/// Is this path or file in a folder named "x"?
		/// </summary>
		public bool InFolder( string folderName, string relative = "game://" )
		{
			var path = Files.Pathing( Output ).Relative( relative ).Output.Contains( folderName );
			return path;
		}

		/// <summary>
		/// Checks if the current path is rooted
		/// </summary>
		public bool IsRelative()
		{
			try
			{
				// Try Catch, just in-case of invalid chars
				return !Path.IsPathRooted( Output );
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Checks if the current path is a full valid path
		/// </summary>
		public bool IsAbsolute()
		{
			try
			{
				// Try Catch, just in-case of invalid chars
				return Path.GetFullPath( Output ) == Output;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Will check if the File or Directory exists
		/// </summary>
		public bool Exists()
		{
			var path = Files.Pathing( Output ).Absolute();
			return Directory.Exists( path ) || File.Exists( path );
		}

		/// <summary>
		/// Gets all directories or files at the given path with the
		/// following search option
		/// </summary>
		public IEnumerable<Pathing> All( bool directories = false, SearchOption option = SearchOption.AllDirectories )
		{
			var pathing = new Pathing( Output ).Absolute();
			return !pathing.Exists() ? Array.Empty<Pathing>() : (directories ? Directory.GetDirectories( pathing.Output, "*", option ) : Directory.GetFiles( pathing.Output, "*", option )).Select( e => new Pathing( e ) );
		}

		/// <summary>
		/// Gets all files at the current scoped directory with the given extensions 
		/// </summary>
		public IEnumerable<Pathing> All( SearchOption option = SearchOption.AllDirectories, params string[] extension )
		{
			var pathing = new Pathing( Output ).Absolute();

			return !pathing.Exists()
				? Array.Empty<Pathing>()
				: Directory.GetFiles( pathing.Output, "*.*", option ).Where( file => Path.HasExtension( file ) && extension.Contains( Path.GetExtension( file )[1..] ) ).Select( e => new Pathing( e ) );

		}

		/// <summary>
		/// Fast Hashes this path for use over network
		/// </summary>
		public int Hash()
		{
			return Output.Hash();
		}

		/// <summary>
		/// Gets the name of last directory or file
		/// at the given path
		/// </summary>
		public string Name( bool withExtension = true )
		{
			return withExtension ? Path.GetFileName( Output ) : Path.GetFileNameWithoutExtension( Output );
		}

		/// <summary>
		/// Gets the FileSystemInfo type
		/// </summary>
		public T Info<T>() where T : FileSystemInfo
		{
			return Meta().IsDirectory ? new DirectoryInfo( Output ) as T : new FileInfo( Output ) as T;
		}
	}
}
