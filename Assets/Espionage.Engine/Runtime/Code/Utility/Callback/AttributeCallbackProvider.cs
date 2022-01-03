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
		private Dictionary<Type, List<object>> _registered = new Dictionary<Type, List<object>>();

		public Task Initialize()
		{
			return Task.Run( () =>
			{
				// Get every Callback using Linq
				var methods = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany( e => e.GetTypes()
									.SelectMany( e => e.GetMethods( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic )
									.Where( e => e.IsDefined( typeof( CallbackAttribute ) ) ) ) );

				foreach ( var item in methods )
				{
					var attribute = item.GetCustomAttribute<CallbackAttribute>();

					if ( !_callbacks.ContainsKey( attribute.Name ) )
					{
						_callbacks.Add( attribute.Name, new List<CallbackInfo>() );
					}

					_callbacks.TryGetValue( attribute.Name, out var items );
					items.Add( new CallbackInfo() { Name = attribute.Name, Info = item, Class = item.DeclaringType } );
				}
			} );
		}

		public void Run( string name, params object[] args )
		{
			if ( !_callbacks.ContainsKey( name ) )
			{
				Debugging.Log.Error( $"No callback for name {name}" );
				return;
			}

			var callbacks = _callbacks[name];
			foreach ( var callback in callbacks )
			{
				if ( callback.IsStatic )
				{
					callback.Info.Invoke( null, args );
					continue;
				}

				if ( _registered.ContainsKey( callback.Class ) )
				{
					foreach ( var obj in _registered[callback.Class] )
					{
						callback.Info?.Invoke( obj, args );
					}
				}
			}
		}

		public void Register( object item )
		{
			if ( _registered.TryGetValue( item.GetType(), out var all ) )
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
	}
}
