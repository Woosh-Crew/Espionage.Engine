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

			return Library.Database.Create<T>( library.Info );
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
