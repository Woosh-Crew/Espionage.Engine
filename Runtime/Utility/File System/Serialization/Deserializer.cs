using System.IO;

namespace Espionage.Engine.IO
{
	public class Deserializer
	{
		protected static IDeserializer<T> GrabDeserializer<T>()
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


	}
}
