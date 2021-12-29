using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class TimedScope : IDisposable
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
		Debug.Log( $"{_message} | {_stopwatch.ElapsedMilliseconds}ms" );
	}
}
