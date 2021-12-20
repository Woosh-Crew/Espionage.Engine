// Attribute based event callback system
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Espionage.Engine
{
	public static class Event
	{
		internal struct EventCallback
		{
			public Type Class { get; set; }
			public MethodInfo Info { get; set; }
		}

		public static void Register( object item )
		{

		}
	}
}
