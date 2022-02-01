using System;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine.Internal
{
	public static class Utility
	{
		private static readonly string[] IgnoredAssemblies =
		{
			"Unity",
			"System",
			"mscorlib",
			"netstandard",
			"Facepunch.Steamworks",
			"ExCSS",
			"Bee.BeeDriver",
			"nunit.framework",
			"PlayerBuildProgram",
			"Mono",
			"JetBrains",
			"Report"
		};

		public static bool IgnoreIfNotUserGeneratedAssembly( Assembly assembly )
		{
			return IgnoredAssemblies.All( item => !assembly.FullName.StartsWith( item ) );
		}
	}
}
