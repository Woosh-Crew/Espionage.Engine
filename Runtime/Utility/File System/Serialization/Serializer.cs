using System.IO;

namespace Espionage.Engine.IO
{
	public class Serializer
	{
		protected static T Find<T>() where T : class, ILibrary
		{
			var library = Library.Database.Find<T>();

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Descriptors for this File" );
			}

			return Library.Database.Create<T>( library.Class );
		}

		//
		// API
		//

		// Serialization

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public virtual void Store( byte[] data, string path )
		{
			path = Files.Pathing.Absolute( path );

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
		public virtual byte[] Serialize<T>( T data )
		{
			var serializer = Find<ISerializer<T>>();
			return serializer.Serialize( data );
		}

		/// <summary>
		/// Serialize an type array of T to a byte array.
		/// </summary>
		public virtual byte[] Serialize<T>( T[] data )
		{
			var serializer = Find<ISerializer<T>>();
			return serializer.Serialize( data );
		}

		// Deserialization

		/// <summary>
		/// Load and deserialize the data for us. Will try and find
		/// the IFile that contains the respective extension.
		/// </summary>
		public T Load<T>( string path ) where T : class, IFile
		{
			// Get the actual path
			path = Files.Pathing.Absolute( path );

			if ( !Files.Pathing.Exists( path ) )
			{
				throw new FileLoadException( "File doesn't exist" );
			}

			var fileInfo = new FileInfo( path );
			var library = Library.Database.Find<T>( e => e.Components.Get<FileAttribute>()?.Extension == fileInfo.Extension[1..] );

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Loaders for this File" );
			}

			var file = Library.Database.Create<T>( library.Class );

			file.File = fileInfo;

			using FileStream stream = new( path, FileMode.Open, FileAccess.Read );
			file.Load( stream );

			return file;
		}

		/// <summary>
		/// Deserializes data at the given path. Will
		/// automatically deserialize it to the target
		/// format.
		/// </summary>
		public virtual T Deserialize<T>( string path )
		{
			path = Files.Pathing.Absolute( path );

			var deserializer = Find<IDeserializer<T>>();
			return deserializer.Deserialize( Deserialize( path ) );
		}

		/// <summary>
		/// Just gives us the raw data from a file at a path
		/// </summary>
		public virtual byte[] Deserialize( string path )
		{
			path = Files.Pathing.Absolute( path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return File.ReadAllBytes( path );
		}
	}
}
