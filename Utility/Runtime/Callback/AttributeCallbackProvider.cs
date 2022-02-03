using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Callbacks
{
	internal class AttributeCallbackProvider : ICallbackProvider
	{
		private Dictionary<string, CallbackInfo.Group> _callbacks = new();
		private Dictionary<Type, List<object>> _registered = new();

		public AttributeCallbackProvider()
		{
			// Select all types where ILibrary exists or if it has the correct attribute
			foreach ( var assembly in AppDomain.CurrentDomain.GetAssemblies() )
			{
				if ( !Utility.IgnoreIfNotUserGeneratedAssembly( assembly ) )
				{
					continue;
				}

				foreach ( var type in assembly.GetTypes() )
				{
					if ( !(type.IsAbstract && type.IsSealed || type.HasInterface<ICallbacks>()) || Utility.IgnoredNamespaces.Any( e => e == type.Namespace ) )
					{
						continue;
					}

					foreach ( var method in type.GetMethods( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy ) )
					{
						var attributes = method.GetCustomAttributes<CallbackAttribute>();

						foreach ( var attribute in attributes )
						{
							if ( !_callbacks.ContainsKey( attribute.Name ) )
							{
								_callbacks.Add( attribute.Name, new CallbackInfo.Group() );
							}

							_callbacks.TryGetValue( attribute.Name, out var items );
							items?.Add( new CallbackInfo { IsStatic = method.IsStatic }.FromType( method.DeclaringType )
								.WithCallback( Build( method ) ) );
						}
					}
				}
			}
		}

		public object[] Run( string name, params object[] args )
		{
			if ( !_callbacks.ContainsKey( name ) )
			{
				return null;
			}

			var callbacks = _callbacks[name];

			// Build the final object array
			var builder = new List<object>();

			foreach ( var callback in callbacks )
			{
				// If the callback is a static method
				// Then just pass in null for the invoke

				if ( callback.IsStatic )
				{
					var arg = callback.Invoke( null, args );

					if ( arg is not null )
					{
						builder.Add( arg );
					}

					continue;
				}

				// If the callback is from an instance, get all instances
				// And invoke them, using the stored object from _registered

				if ( !_registered.ContainsKey( callback.Class ) )
				{
					continue;
				}

				var targets = _registered[callback.Class];
				builder.AddRange( from item in targets where ((ICallbacks)item).CanCallback( name ) select callback.Invoke( item, args ) );
			}

			return builder.ToArray();
		}

		public void Register( object item )
		{
			var type = item.GetType();

			if ( !_registered.ContainsKey( type ) )
			{
				_registered.Add( type, new List<object>() );
			}

			if ( _registered.TryGetValue( type, out var all ) )
			{
				all.Add( item );
			}
		}

		public void Unregister( object item )
		{
			if ( _registered.TryGetValue( item.GetType(), out var all ) )
			{
				all.Remove( item );
			}
		}

		public void Dispose()
		{
			_registered?.Clear();
			_registered = null;

			_callbacks?.Clear();
			_callbacks = null;

			Debugging.Log.Warning( "Disposing ICallbackProvider" );
		}

		private static CallbackInfo.Action Build( MethodBase info )
		{
			return ( target, args ) => info?.Invoke( target, args );
		}
	}
}
