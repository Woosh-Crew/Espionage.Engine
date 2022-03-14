using System.IO;

namespace Espionage.Engine.IO
{
	public class Deserializer
	{
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
		// API
		//

		/// <summary>
		/// Deserializes data at the given path. Will
		/// automatically deserialize it to the target
		/// format.
		/// </summary>
		public T Deserialize<T>( string path )
		{
			path = Files.Pathing.Get( path );

			var deserializer = GrabDeserializer<T>();
			return deserializer.Deserialize( Deserialize( path ) );
		}

		/// <summary>
		/// Just gives us the raw data from a file at a path
		/// </summary>
		public byte[] Deserialize( string path )
		{
			path = Files.Pathing.Get( path );

			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException();
			}

			return File.ReadAllBytes( path );
		}
	}
}
