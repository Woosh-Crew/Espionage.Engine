using UnityEngine;

namespace Espionage.Engine.Logging
{
	public struct Entry
	{
		public float Time { get; set; }
		public Color Color { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public string Level { get; set; }
	}
}
