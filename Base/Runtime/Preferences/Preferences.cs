using System.Collections;
using UnityEngine;

namespace Espionage.Engine
{
	public static partial class Preferences
	{
		public static IDatabase<string, Item> Database { get; set; }

		public class Item
		{
			public string Name { get; set; }
			public string Title { get; set; }
			public string Tooltip { get; set; }
			public string Group { get; set; }
		}
	}
}
