using System;

namespace Espionage.Engine.Internal.Logging
{
	[Serializable]
	public struct Entry
	{
		public string Message;
		public string StackTrace;
		public Level Type;

		public enum Level { Info, Warning, Error, Exception }
	}
}
