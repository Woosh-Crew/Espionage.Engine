using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espionage.Engine.Internal;

namespace Espionage.Engine.Internal
{
	public interface ILoggingProvider
	{
		void Initialize() { return; }

		Action<Entry> OnLogged { get; set; }

		void Add( Entry entry );
		void Clear();

		IReadOnlyCollection<Entry> All { get; }
	}
}

//
// Extensions
// 

public static class ILoggingProviderExtensions
{
	public static void Info( this ILoggingProvider provider, object message )
	{
		provider.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
		} );
	}

	public static void Warning( this ILoggingProvider provider, object message )
	{
		provider.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
		} );
	}

	public static void Error( this ILoggingProvider provider, object message )
	{
		provider.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
		} );
	}
}
