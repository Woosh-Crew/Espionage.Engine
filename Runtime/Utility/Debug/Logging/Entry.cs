using System;
using UnityEngine;

namespace Espionage.Engine.Logging
{
	public struct Entry
	{
		public DateTime Time { get; set; }
		public Color Color { get; set; }
		public string Message { get; set; }
		public string Trace { get; set; }
		public string Level { get; set; }
	}
}
