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

		public void Add( Function function )
		{
			var attributes = function.Info.GetCustomAttributes<CallbackAttribute>();

			foreach ( var attribute in attributes )
			{
				if ( !_callbacks.ContainsKey( attribute.Name ) )
				{
					_callbacks.Add( attribute.Name, new CallbackInfo.Group() );
				}

				_callbacks.TryGetValue( attribute.Name, out var items );
				items?.Add( new CallbackInfo { IsStatic = function.Info.IsStatic }.FromType( function.Info.DeclaringType )
					.WithCallback( Build( function.Info ) ) );
			}
		}

		public void Run( string name )
		{
			if ( !_callbacks.TryGetValue( name, out var callbacks ) )
			{
				return;
			}

			for ( var index = 0; index < callbacks.Count; index++ )
			{
				var callback = callbacks[index];
				if ( callback.IsStatic )
				{
					callback.Invoke( null, null );
					continue;
				}

				// If the callback is from an instance, get all instances
				// And invoke them, using the stored object from _registered
				if ( !_registered.ContainsKey( callback.Class ) )
				{
					continue;
				}

				var targets = _registered[callback.Class];
				for ( var targetIndex = 0; targetIndex < targets.Count; targetIndex++ )
				{
					var item = targets[targetIndex];
					callback.Invoke( item );
				}
			}
		}

		public object[] Run( string name, params object[] args )
		{
			if ( !_callbacks.TryGetValue( name, out var callbacks ) )
			{
				return null;
			}

			// Build the final object array
			var builder = new List<object>();

			for ( var index = 0; index < callbacks.Count; index++ )
			{
				var callback = callbacks[index];
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
				builder.AddRange( targets.Select( item => callback.Invoke( item, args ) ) );
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
