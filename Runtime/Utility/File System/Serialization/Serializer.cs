using System.IO;

namespace Espionage.Engine.IO
{
	public class Serializer
	{
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
		// API
		//

		/// <summary>
		/// Saves a byte buffer to a path,
		/// it will overwrite if the file at that
		/// path already exists.
		/// </summary>
		public void Store( byte[] data, string path )
		{
			path = Files.Pathing.Get( path );

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
		public byte[] Serialize<T>( T data )
		{
			var serializer = GrabSerializer<T>();
			return serializer.Serialize( data );
		}

		/// <summary>
		/// Serialize an type array of T to a byte array.
		/// </summary>
		public byte[] Serialize<T>( T[] data )
		{
			var serializer = GrabSerializer<T>();
			return serializer.Serialize( data );
		}
	}
}
