// Attribute based event callback system

using System;
using System.Reflection;

namespace Espionage.Engine.Internal.Callbacks
{
	internal struct CallbackInfo
	{
		public string Name { get; internal set; }
		public Type Class { get; internal set; }
		public MethodInfo Info { get; internal set; }

		// Helpers
		public bool IsStatic => Info.IsStatic;
	}
}
