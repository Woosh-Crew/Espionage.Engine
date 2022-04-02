using System;
using UnityEngine;

namespace Espionage.Engine
{
	public class Interactor : Component<Actor>, ISimulated
	{
		[Slider( 0.1f, 2 )]
		public float Length { get; set; } = 0.9f;

		// Current

		public IUsable Using { get; private set; }
		public IHoverable Hovering { get; private set; }

		// Logic

		public void Simulate( Client cl )
		{
			const KeyCode interact = KeyCode.E;

			// IHoverable

			Hovering = Find<IHoverable>( 0.2f, e => e.Show( Entity ) );

			// IUsable

			if ( Input.GetKeyDown( interact ) )
			{
				Start();
			}

			if ( !Input.GetKey( interact ) )
			{
				Stop();
				return;
			}

			// To far away, stop using
			if ( Vector3.Distance( _positionWhenUsed, Entity.Position ) > 0.5f )
			{
				Stop();
			}

			if ( Using == null || !Using.OnUse( Entity ) )
			{
				return;
			}

			Stop();
		}

		// IUse

		public event Action OnFailed;

		private Vector3 _positionWhenUsed;

		private void Start()
		{
			Using = Find<IUsable>( 0.2f, e => e.IsUsable( Entity ) );
			Using?.Started( Entity );

			_positionWhenUsed = Entity.Position;

			if ( Using == null )
			{
				Failed();
			}
		}

		private void Stop()
		{
			Using?.Stopped( Entity );
			Using = null;
			_positionWhenUsed = default;
		}

		protected virtual void Failed()
		{
			// Play a sound?
			OnFailed?.Invoke();
		}

		// Helpers

		private T Find<T>( float size, Func<T, bool> canUse ) where T : class
		{
			var ray = Trace.Ray( Entity.Eyes.Position, Entity.Eyes.Rotation.Forward(), Length ).Ignore( "Pawn" );
			var entity = ray.Run<T>() ?? ray.Radius( size ).Run<T>();

			// Set root ray to larger size then run
			if ( entity == null || !canUse.Invoke( entity ) )
			{
				return null;
			}

			return entity;
		}
	}
}
