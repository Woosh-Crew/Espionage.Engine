using System;
using System.Diagnostics;
using System.IO;
using Espionage.Engine.IO;

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
		public static Pathing Pathing { get; } = new();
		public static Serializer Serialization { get; } = new();

		//
		// API
		//

		/// <summary>
		/// Gets the class that represents the target files extension.
		/// </summary>
		public static T Grab<T>( string path, bool load = true ) where T : class, IFile
		{
			path = Pathing.Absolute( path );
			Assert.IsFalse( Pathing.Exists( path ), "File doesn't exist" );

			var info = new FileInfo( path );
			var library = Library.Database.Find<T>( e => e.Components.Get<FileAttribute>()?.Extension == info.Extension[1..] );

			Assert.IsNull( library, "No Valid Loaders for this File" );

			var file = Library.Database.Create<T>( library.Info );
			file.Info = info;

			return file;
		}

		/// <inheritdoc cref="Save{T}(T,string)"/>
		public static void Save<T>( Library lib, T item, string path )
		{
			Serialization.Store( Serialization.Serialize( lib, item ), path );
		}

		/// <summary>
		/// Saves anything you want, (provided theres a
		/// serializer for it) to the given path
		/// </summary>
		public static void Save<T>( T item, string path )
		{
			Serialization.Store( Serialization.Serialize( item ), path );
		}

		/// <inheritdoc cref="Save{T}(T,string)"/>
		public static void Save<T>( Library lib, T[] item, string path )
		{
			Serialization.Store( Serialization.Serialize( lib, item ), path );
		}

		/// <summary>
		/// Saves an array of anything you want,
		/// (provided theres a serializer for it)
		/// to the given path
		/// </summary>
		public static void Save<T>( T[] item, string path )
		{
			Serialization.Store( Serialization.Serialize( item ), path );
		}

		/// <summary>
		/// Deletes the file at the given path
		/// </summary>
		public static void Delete( string path )
		{
			path = Pathing.Absolute( path );

			if ( File.Exists( path ) )
			{
				File.Delete( path );
			}

			Dev.Log.Error( $"File [{path}], couldn't be deleted." );
		}

		/// <summary>
		/// Deletes all files with the given extension at the path
		/// </summary>
		public static void Delete( string path, string extension )
		{
			path = Pathing.Absolute( path );

			var files = Directory.GetFiles( path, $"*.{extension}" );
			foreach ( var item in files )
			{
				File.Delete( item );
			}
		}

		/// <inheritdoc cref="Delete(string, string)"/> 
		public static void Delete( string path, params string[] extension )
		{
			if ( !Pathing.Exists( path ) )
			{
				Dev.Log.Error( $"Path [{path}], doesn't exist" );
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
		public static void Copy( string file, string path, bool overwrite = true )
		{
			file = Pathing.Absolute( file );
			path = Pathing.Absolute( path );

			var fileInfo = new FileInfo( file );

			if ( !File.Exists( file ) )
			{
				throw new FileNotFoundException();
			}

			Pathing.Create( path );
			fileInfo.CopyTo( path + $"{Pathing.Name( file )}", overwrite );
		}

		/// <summary>
		/// Moves the source file to the target destination
		/// </summary>
		public static void Move( string source, string destination )
		{
			source = Pathing.Absolute( source );
			destination = Pathing.Absolute( destination );

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
			path = Pathing.Absolute( path );

			if ( !Pathing.Exists( path ) )
			{
				Dev.Log.Warning( $"Path or File [{path}], doesn't exist" );
			}

			Process.Start( $"file://{path}" );
		}

		//
		// Structs
		//

		public readonly struct Meta
		{
			internal Meta( FileAttributes attributes, DateTime creation, DateTime access, DateTime modified )
			{
				Attributes = attributes;
				Creation = creation;
				Access = access;
				Modified = modified;
			}

			public FileAttributes Attributes { get; }

			// Helpers

			public bool IsFile => Attributes is not FileAttributes.Directory;
			public bool IsDirectory => Attributes is FileAttributes.Directory;

			// Timings

			public DateTime Creation { get; }
			public DateTime Access { get; }
			public DateTime Modified { get; }
		}
	}
}
