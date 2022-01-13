using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Callbacks
{
	internal class AttributeCallbackProvider : ICallbackProvider
	{
		private static Dictionary<string, CallbackInfo.Group> _callbacks = new Dictionary<string, CallbackInfo.Group>();
		private static Dictionary<Type, List<object>> _registered = new Dictionary<Type, List<object>>();

		public Task Initialize()
		{
			return Task.Run( () =>
			{
				// Get every Callback using Linq
				var methods = AppDomain.CurrentDomain.GetAssemblies()
						.Where( e => Utility.IgnoreIfNotUserGeneratedAssembly( e ) )
						.SelectMany( e => e.GetTypes()
							// We gotta do this so it loads faster... I think its stupid having to have a library attribute
							.SelectMany( e => e.GetMethods( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy ) ) );

				foreach ( var info in methods )
				{
					var attribute = info.GetCustomAttribute<CallbackAttribute>();

					if ( attribute is null )
						continue;

					if ( !_callbacks.ContainsKey( attribute.Name ) )
					{
						_callbacks.Add( attribute.Name, new CallbackInfo.Group() );
					}

					_callbacks.TryGetValue( attribute.Name, out var items );
					items.Add( new CallbackInfo() { IsStatic = info.IsStatic }.FromType( info.DeclaringType ).WithCallback( Build( info ) ) );
				}
			} );
		}

		internal CallbackInfo.Action Build( MethodInfo info )
		{
			return delegate ( object target, object[] args )
			{
				return info?.Invoke( target, args );
			};
		}

		public object[] Run( string name, params object[] args )
		{
			if ( !_callbacks.ContainsKey( name ) )
				return null;

			var callbacks = _callbacks[name];

			// Build the final object array
			List<object> builder = new List<object>();

			foreach ( var callback in callbacks )
			{
				// If the callback is a static method
				// Then just pass in null for the invoke

				if ( callback.IsStatic )
				{
					var arg = callback.Invoke( null, args );

					if ( arg is not null )
						builder.Add( arg );

					continue;
				}

				// If the callback is from an instance, get all instances
				// And invoke them, using the stored object from _registered

				if ( _registered.ContainsKey( callback.Class ) )
				{
					foreach ( var obj in _registered[callback.Class] )
					{
						var arg = callback.Invoke( obj, args );

						if ( arg is not null )
							builder.Add( arg );
					}
				}
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

			Debugging.Log.Warning( "Dispoing ICallbackProvider" );
		}
	}
}
