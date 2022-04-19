using System;
using UnityEngine;

namespace Espionage.Engine
{
	[Singleton]
	public class Interactor : Component<Actor>, ISimulated
	{
		public float Length { get; set; } = 1.25f;
		public IUsable Using { get; private set; }
		public IHoverable Hovering { get; private set; }
		public Binding Binding { get; set; }

		// Logic

		public void Simulate( Client cl )
		{
			Binding ??= Controls.Scheme["Interact"];

			// IHoverable
			Hovering = Find<IHoverable>( 0.2f, e => e.Show( Entity ), out _ );

			// IUsable
			if ( Binding.Pressed )
			{
				Start();
			}

			if ( !Binding.Down )
			{
				Stop();
				return;
			}

			if ( Using != null && (Vector3.Dot( Entity.Rotation.Forward(), _positionWhenUsed - Entity.Position ) < 0.3f || Vector3.Distance( _positionWhenUsed, Entity.Position ) > 2f) )
			{
				Stop();
				return;
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
			Using = Find<IUsable>( 0.2f, e => e.IsUsable( Entity ), out var info );
			Using?.Started( Entity );

			_positionWhenUsed = info.End;

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

		private T Find<T>( float size, Func<T, bool> canUse, out Trace.Result hit ) where T : class
		{
			var ray = Entity.Eyes.Ray( Length );
			var entity = ray.Run<T>( out var result ) ?? ray.Radius( size ).Run<T>( out result );

			// Set root ray to larger size then run
			if ( entity == null || !canUse.Invoke( entity ) )
			{
				hit = default;
				return null;
			}

			hit = result.Value;
			return entity;
		}
	}
}
