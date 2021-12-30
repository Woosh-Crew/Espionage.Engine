using System;

namespace Espionage.Engine
{
	public static class Debugging
	{
		public static IDisposable Stopwatch( string message = null, params object[] args )
		{
			return new TimedScope( message, args );
		}
	}
}
