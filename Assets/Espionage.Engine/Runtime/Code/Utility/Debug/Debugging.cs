using System;

namespace Espionage.Engine
{
	public static class Debugging
	{
		public static IDisposable Stopwatch( string message = null )
		{
			return new TimedScope( message );
		}
	}
}
