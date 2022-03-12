using System.IO;

namespace Espionage.Engine
{
	public static partial class Files
	{
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
	}
}
