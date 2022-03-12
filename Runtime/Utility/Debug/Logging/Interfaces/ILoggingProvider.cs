using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espionage.Engine.Internal.Logging;

namespace Espionage.Engine.Internal.Logging
{
	public interface ILoggingProvider
	{
		IReadOnlyCollection<Entry> All { get; }
		Action<Entry> OnLogged { get; set; }

		void Add( Entry entry );
		void Clear();
	}
}

//
// Extensions
// 

public static class LoggingProviderExtensions
{
	public static void Debug( this ILoggingProvider provider, object message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Debug
		} );
	}

	public static void Verbose<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Debug
		} );
	}

	public static void Info<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Info
		} );
	}

	public static void Warning<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Warning
		} );
	}

	public static void Error<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Error
		} );
	}

	public static void Exception( this ILoggingProvider provider, Exception exception )
	{
		provider?.Add( new()
		{
			Message = exception.Message,
			StackTrace = Environment.StackTrace,
			Type = Entry.Level.Error
		} );
	}
}
