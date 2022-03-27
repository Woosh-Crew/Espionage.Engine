using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Espionage.Engine.Logging;
using UnityEngine;

namespace Espionage.Engine.Logging
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
	public static void Verbose<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			Trace = Environment.StackTrace,
			Level = "Verbose",
			Color = Color.gray
		} );
	}

	public static void Info<T>( this ILoggingProvider provider, T message, string stack = null )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			Trace = string.IsNullOrWhiteSpace( stack ) ? Environment.StackTrace : stack,
			Level = "Info",
			Color = Color.white
		} );
	}

	public static void Warning<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			Trace = Environment.StackTrace,
			Level = "Warning",
			Color = Color.yellow
		} );
	}

	public static void Error<T>( this ILoggingProvider provider, T message )
	{
		provider?.Add( new()
		{
			Message = message.ToString(),
			Trace = Environment.StackTrace,
			Level = "Error",
			Color = Color.red
		} );
	}

	public static void Exception( this ILoggingProvider provider, Exception exception )
	{
		provider?.Add( new()
		{
			Message = $"{exception.Message}",
			Trace = exception.StackTrace,
			Level = "Exception",
			Color = Color.red
		} );
	}
}
