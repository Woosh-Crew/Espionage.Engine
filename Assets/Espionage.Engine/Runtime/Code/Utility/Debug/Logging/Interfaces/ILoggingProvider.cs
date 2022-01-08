using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espionage.Engine.Internal.Logging;

namespace Espionage.Engine.Internal.Logging
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
		provider?.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
			Type = Entry.Level.Info,
		} );
	}

	public static void Warning( this ILoggingProvider provider, object message )
	{
		provider?.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
			Type = Entry.Level.Warning,
		} );
	}

	public static void Error( this ILoggingProvider provider, object message )
	{
		provider?.Add( new Entry()
		{
			Message = message.ToString(),
			StackTrace = System.Environment.StackTrace,
			Type = Entry.Level.Error,
		} );
	}

	public static void Exception( this ILoggingProvider provider, Exception exception )
	{
		provider?.Add( new Entry()
		{
			Message = exception.Message,
			StackTrace = System.Environment.StackTrace,
			Type = Entry.Level.Error,
		} );
	}
}
