using System;
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
			"Mono"
		};

		public static bool IgnoreIfNotUserGeneratedAssembly( Assembly assembly )
		{
			foreach ( var item in IgnoredAssemblies )
			{
				if ( assembly.FullName.StartsWith( item ) )
					return false;
			}

			return true;
		}
	}
}
