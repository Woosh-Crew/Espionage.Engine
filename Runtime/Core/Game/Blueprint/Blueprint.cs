using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Resources;

namespace Espionage.Engine
{
	public class Blueprint
	{
		[Property]
		public string Entity { get; set; }

		[Property]
		public string[] Components { get; set; }
	}
}
