using System;
using System.Collections.Generic;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public abstract partial class Entity
	{
		public static Builder Setup( string library )
		{
			return new( library );
		}

		public static Builder<T> Setup<T>() where T : Entity
		{
			return new( Library.Database[typeof( T )] );
		}


		public readonly struct Builder
		{
			private readonly Library _lib;
			internal Builder( string name ) : this( Library.Database[name] ) { }

			internal Builder( Library library )
			{
				_lib = library;
				_components = new();
			}

			private readonly List<Type> _components;

			public Builder With<T>() where T : IComponent<Entity>
			{
				if ( _components.Contains( typeof( T ) ) )
				{
					Dev.Log.Warning( $"Builder already contains component, {typeof( T ).FullName}" );
					return this;
				}

				_components.Add( typeof( T ) );
				return this;
			}

			public Entity Build()
			{
				var ent = Library.Create( _lib ) as Entity;

				if ( ent == null )
				{
					Dev.Log.Error( "Entity was null" );
					return null;
				}

				foreach ( var component in _components )
				{
					ent.Components.Create( () => ent.gameObject.AddComponent( component ) );
				}

				return ent;
			}
		}

		public readonly struct Builder<T> where T : Entity
		{
			private readonly Library _lib;
			private readonly List<Type> _components;

			internal Builder( Library library )
			{
				_lib = library;
				_components = new();
			}

			public Builder<T> With<TComp>() where TComp : IComponent<Entity>
			{
				if ( _components.Contains( typeof( TComp ) ) )
				{
					Dev.Log.Warning( $"Builder already contains component, {typeof( TComp ).FullName}" );
					return this;
				}

				_components.Add( typeof( TComp ) );
				return this;
			}

			public T Build()
			{
				var ent = Library.Create( _lib ) as T;

				if ( ent == null )
				{
					Dev.Log.Error( "Entity was null" );
					return null;
				}

				foreach ( var component in _components )
				{
					ent.Components.Create( () => ent.gameObject.AddComponent( component ) );
				}

				return ent;
			}
		}
	}
}
