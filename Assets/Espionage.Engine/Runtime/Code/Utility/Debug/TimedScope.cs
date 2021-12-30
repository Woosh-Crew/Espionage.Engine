using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

internal class TimedScope : IDisposable
{
	private Stopwatch _stopwatch;
	private string _message;
	private object[] _args;

	public TimedScope( string message, params object[] args )
	{
		_stopwatch = Stopwatch.StartNew();
		_message = message;
		_args = args;
	}

	public void Dispose()
	{
		_stopwatch.Stop();

		if ( string.IsNullOrEmpty( _message ) )
		{
			Debug.Log( $"{_stopwatch.ElapsedMilliseconds}ms" );
			return;
		}

		Debug.Log( $"{String.Format( _message, _args )} | {_stopwatch.ElapsedMilliseconds}ms" );
	}
}
