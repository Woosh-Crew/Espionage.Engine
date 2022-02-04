using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	internal static class Manager
	{
		private static void Invoker( Layer layer )
		{
			// Get all manager types - this is the most aids shit ever
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.Where( Utility.IgnoreIfNotUserGeneratedAssembly )
				.SelectMany( e => e.GetTypes()
					.Where( type =>
					{
						if ( Utility.IgnoredNamespaces.Any( s => s == type.Namespace ) )
						{
							return false;
						}

						var attribute = type.GetCustomAttribute<ManagerAttribute>();

						if ( attribute is null )
						{
							return false;
						}

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
				var method = item.GetMethod( item.GetCustomAttribute<ManagerAttribute>().Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic );
				method?.Invoke( null, null );
			}
		}

		//
		// Runtime
		//

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
		private static void InvokeRuntime()
		{
			using ( Debugging.Stopwatch( "Runtime Layer Initialized" ) )
			{
				Invoker( Layer.Runtime );
			}
		}

		//
		// Editor
		//

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
		private static void InvokeEditor()
		{
			using ( Debugging.Stopwatch( "Editor Layer Initialized" ) )
			{
				Invoker( Layer.Editor );
			}
		}
#endif
	}
}
