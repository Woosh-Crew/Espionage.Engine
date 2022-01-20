using System;
using System.Linq;
using System.Reflection;

namespace Espionage.Engine.Internal
{
	public static class Utility
	{
		private static readonly string[] IgnoredAssemblies = new string[]
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
			if ( IgnoredAssemblies.All( item => !assembly.FullName.StartsWith( item ) ) )
			{
				Debugging.Log.Info( assembly.FullName );
				return true;
			}

			return false;
		}
	}
}
