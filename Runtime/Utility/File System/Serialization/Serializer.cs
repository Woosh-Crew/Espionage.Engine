using System.IO;

namespace Espionage.Engine.IO
{
	public class Serializer
	{
		//
		// API
		//

		// Serialization

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public virtual void Store( byte[] data, Pathing path )
		{
			var fileInfo = new FileInfo( path.Absolute() );
			Files.Pathing( fileInfo.DirectoryName ).Create();

			using var stream = File.Create( path );
			stream.Write( data );
		}

		/// <summary>
		/// Serialize type of T to a byte array.
		/// </summary>
		public virtual byte[] Serialize<T>( T data )
		{
			return Serialize( Library.Database.Find<ISerializer<T>>(), data );
		}

		/// <summary>
		/// <para>
		/// <inheritdoc cref="Serialize{T}(T)"/>
		/// </para>
		/// <para>
		/// This is faster, if you already have the lib.
		/// </para>
		/// </summary>
		public virtual byte[] Serialize<T>( Library lib, T data )
		{
			var serializer = Library.Create<ISerializer<T>>( lib.Info );
			return serializer.Serialize( data );
		}

		/// <summary>
		/// Serialize an type array of T to a byte array.
		/// </summary>
		public virtual byte[] Serialize<T>( T[] data )
		{
			return Serialize( Library.Database.Find<ISerializer<T>>(), data );
		}

		/// <summary>
		/// <para>
		/// <inheritdoc cref="Serialize{T}(T)"/>
		/// </para>
		/// <para>
		/// This is faster, if you already have the lib.
		/// </para>
		/// </summary>
		public virtual byte[] Serialize<T>( Library lib, T[] data )
		{
			var serializer = Library.Create<ISerializer<T>>( lib.Info );
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
			return Deserialize<T>( Library.Database.Find<IDeserializer<T>>(), path );
		}

		/// <summary>
		/// <para>
		/// <inheritdoc cref="Deserialize{T}(string)"/>
		/// </para>
		/// <para>
		/// This is faster, if you already have the lib.
		/// </para>
		/// </summary>
		public virtual T Deserialize<T>( Library lib, Pathing path )
		{
			path.Absolute();
			var deserializer = Library.Create<IDeserializer<T>>( lib.Info );
			return deserializer.Deserialize( Deserialize( path ) );
		}

		/// <summary>
		/// Just gives us the raw data from a file at a path
		/// </summary>
		public virtual byte[] Deserialize( Pathing path )
		{
			path.Absolute();

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return File.ReadAllBytes( path );
		}
	}
}
