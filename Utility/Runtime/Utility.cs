using System;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine.Internal
{
	public static class Utility
	{
		//
		// Helpers
		//

		public static string[] IgnoredAssemblies { get; } =
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

		public static string[] IgnoredNamespaces { get; } = { "DiscordAPI", "Steamworks", "UnityEngine" };

		public static bool IgnoreIfNotUserGeneratedAssembly( Assembly assembly )
		{
			return IgnoredAssemblies.All( item => !assembly.FullName.StartsWith( item ) );
		}
	}
}
