using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	internal sealed class Manager
	{
		private static void Invoker( Layer layer )
		{
			// Get all manager types - this is the most aids shit ever
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany( e => e.GetTypes()
								.Where( e =>
								{
									var attribute = e.GetCustomAttribute<ManagerAttribute>();

									if ( attribute is null )
										return false;

									if ( attribute.Layer == (Layer.Runtime | Layer.Editor) )
									{
										return layer == Layer.Editor;
									}

									return attribute.Layer.HasFlag( layer );
								} ) )
								.OrderBy( e => e.GetCustomAttribute<ManagerAttribute>().Order );

			// Invoke all init methods - or cache if it returns task
			foreach ( var item in types )
			{
				var method = item.GetMethod( item.GetCustomAttribute<ManagerAttribute>().Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic );
				method?.Invoke( null, null );
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
