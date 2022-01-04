using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Callbacks
{
	public class AttributeCallbackProvider : ICallbackProvider
	{
		private static Dictionary<string, List<CallbackInfo>> _callbacks = new Dictionary<string, List<CallbackInfo>>();
		private static Dictionary<Type, List<object>> _registered = new Dictionary<Type, List<object>>();

		public Task Initialize()
		{
			return Task.Run( () =>
			{
				// Get every Callback using Linq
				var methods = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany( e => e.GetTypes()
									.SelectMany( e => e.GetMethods( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy )
									.Where( e => e.IsDefined( typeof( CallbackAttribute ) ) ) ) );

				foreach ( var info in methods )
				{
					var attribute = info.GetCustomAttribute<CallbackAttribute>();

					if ( !_callbacks.ContainsKey( attribute.Name ) )
					{
						_callbacks.Add( attribute.Name, new List<CallbackInfo>() );
					}

					_callbacks.TryGetValue( attribute.Name, out var items );
					items.Add( new CallbackInfo() { IsStatic = info.IsStatic }.FromType( info.DeclaringType ).WithCallback( Build( info ) ) );
				}
			} );
		}

		internal CallbackInfo.CallbackEvent Build( MethodInfo info )
		{
			return delegate ( object target, object[] args )
			{
				info?.Invoke( target, args );
			};
		}

		public void Run( string name, params object[] args )
		{
			if ( !_callbacks.ContainsKey( name ) )
				return;

			var callbacks = _callbacks[name];
			foreach ( var callback in callbacks )
			{
				Debugging.Log.Info( "Calling" );

				if ( callback.IsStatic )
				{
					callback.Invoke( null, args );
					continue;
				}

				if ( _registered.ContainsKey( callback.Class ) )
				{
					foreach ( var obj in _registered[callback.Class] )
					{
						callback.Invoke( obj, args );
					}
				}
			}
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
