using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Logging
{
	public struct Entry
	{
		public static Dictionary<Level, Color> Colors { get; } = new()
		{
			[Level.Debug] = Color.white,
			[Level.Info] = Color.white,
			[Level.Warning] = Color.yellow,
			[Level.Error] = Color.red,
			[Level.Exception] = Color.red
		};

		public string Message;
		public string StackTrace;
		public Level Type;

		public enum Level { Debug, Info, Warning, Error, Exception }
	}
}
