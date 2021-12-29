using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

internal class TimedScope : IDisposable
{
	private Stopwatch _stopwatch;
	private string _message;

	public TimedScope( string message )
	{
		_stopwatch = Stopwatch.StartNew();
		_message = message;
	}

	public void Dispose()
	{
		_stopwatch.Stop();

		if ( string.IsNullOrEmpty( _message ) )
		{
			Debug.Log( $"{_stopwatch.ElapsedMilliseconds}ms" );
			return;
		}

		Debug.Log( $"{_message} | {_stopwatch.ElapsedMilliseconds}ms" );
	}
}
