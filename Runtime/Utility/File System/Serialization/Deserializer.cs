using System.IO;

namespace Espionage.Engine.IO
{
	public class Deserializer
	{
		private IDeserializer<T> GrabDeserializer<T>()
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
		/// Load and deserialize the data for us. Will try and find
		/// the IFile that contains the respective extension.
		/// </summary>
		public T Load<T>( string path ) where T : class, IFile
		{
			// Get the actual path
			path = Files.Pathing.Get( path );

			if ( !File.Exists( path ) )
			{
				throw new FileLoadException( "File doesn't exist" );
			}

			var fileInfo = new FileInfo( path );

			var library = Library.Database.Find<T>( e => e.Components.Get<FileAttribute>()?.Extension == fileInfo.Extension[1..] );

			if ( library == null )
			{
				throw new FileLoadException( "No Valid Deserializers for this File" );
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
