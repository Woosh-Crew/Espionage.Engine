using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	internal sealed class Manager
	{
		private static void Invoker( Layer layer )
		{
			// Get all manager types
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany( e => e.GetTypes()
								.Where( e => e.IsDefined( typeof( ManagerAttribute ), false ) && e.GetCustomAttribute<ManagerAttribute>().Layer.HasFlag( layer ) ) );

			foreach ( var item in types )
			{
				item.GetMethod( item.GetCustomAttribute<ManagerAttribute>().Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic )?.Invoke( null, null );
			}
		}

		//
		// Runtime
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.AfterAssembliesLoaded )]
		private static void InvokeRuntime() => Invoker( Layer.Runtime );

		//
		// Editor
		//

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		private static void InvokeEditor() => Invoker( Layer.Editor );
#endif
	}
}
