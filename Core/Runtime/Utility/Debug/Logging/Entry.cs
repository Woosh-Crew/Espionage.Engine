using System;

namespace Espionage.Engine.Internal.Logging
{
	public struct Entry
	{
		public string Message;
		public string StackTrace;
		public Level Type;

		public enum Level { Debug, Info, Warning, Error, Exception }
	}
}
