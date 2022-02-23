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
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.Where( e => e == typeof( Library ).Assembly || e.GetReferencedAssemblies().Any( assemblyName => assemblyName.Name == typeof( Library ).Assembly.GetName().Name ) )
				.SelectMany( e => e.GetTypes()
					.Where( type =>
					{
						if ( !type.IsDefined( typeof( ManagerAttribute ) ) )
						{
							return false;
						}

						var attribute = type.GetCustomAttribute<ManagerAttribute>();
						if ( attribute.Layer == (Layer.Runtime | Layer.Editor) )
						{
							return layer == Layer.Editor;
						}

						return attribute.Layer.HasFlag( layer );
					} ) )
				.OrderBy( e => e.GetCustomAttribute<ManagerAttribute>().Order );

			foreach ( var item in types )
			{
				item.GetMethod( item.GetCustomAttribute<ManagerAttribute>().Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )?.Invoke( null, null );
			}
		}

		[RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
		private static void InvokeRuntime()
		{
			using ( Debugging.Stopwatch( "Runtime Layer Initialized" ) )
			{
				Invoker( Layer.Runtime );
				Callback.Run( "manager.runtime_ready" );
			}
		}

	#if UNITY_EDITOR

		[UnityEditor.InitializeOnLoadMethod]
		private static void InvokeEditor()
		{
			using ( Debugging.Stopwatch( "Editor Layer Initialized" ) )
			{
				Invoker( Layer.Editor );
				Callback.Run( "manager.editor_ready" );
			}
		}

	#endif
	}
}
