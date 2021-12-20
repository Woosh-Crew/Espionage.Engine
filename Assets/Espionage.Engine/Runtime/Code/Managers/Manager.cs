using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	internal sealed class Manager
	{
		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterAssembliesLoaded )]
		private static void Cache()
		{
			// Get all manager types
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany( e => e.GetTypes()
								.Where( e => e.IsDefined( typeof( ManagerAttribute ), false ) && e.GetCustomAttribute<ManagerAttribute>().Layer == Layer.Runtime ) );
		}
	}
}
