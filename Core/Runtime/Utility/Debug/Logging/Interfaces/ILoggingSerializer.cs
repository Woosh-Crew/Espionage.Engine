using System.Collections.Generic;

namespace Espionage.Engine.Internal.Logging
{
	public interface ILoggingSerializer
	{
		void Serialize( List<Entry> entries );
		List<Entry> Deserialize();
	}
}
