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

		public Task Initialize()
		{
			{
				// Get every Callback using Linq
				var methods = AppDomain.CurrentDomain.GetAssemblies()
					.Where( Utility.IgnoreIfNotUserGeneratedAssembly )
					.SelectMany( e => e.GetTypes().Where( type => type.IsAbstract && type.IsSealed || type.HasInterface<ICallbacks>() )
						.SelectMany( type => type.GetMethods( BindingFlags.Instance | BindingFlags.Static |
						                                      BindingFlags.Public | BindingFlags.NonPublic |
						                                      BindingFlags.FlattenHierarchy ) ) );

				foreach ( var info in methods )
				{
					var attributes = info.GetCustomAttributes<CallbackAttribute>();

					foreach ( var attribute in attributes )
					{
						if ( !_callbacks.ContainsKey( attribute.Name ) )
						{
							_callbacks.Add( attribute.Name, new CallbackInfo.Group() );
						}

						_callbacks.TryGetValue( attribute.Name, out var items );
						items?.Add( new CallbackInfo { IsStatic = info.IsStatic }.FromType( info.DeclaringType )
							.WithCallback( Build( info ) ) );
					}
				}
			}

			return Task.CompletedTask;
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
