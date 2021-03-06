namespace Espionage.Engine
{
	public class Pushing : Component<Actor>, ISimulated
	{
		public void Simulate( Client client )
		{
			var wheel = client.Input.Mouse.Wheel;

			if ( wheel != 0 )
			{
				Push( wheel );
			}
		}

		public virtual void Push( float mouseDelta, float size = 0 )
		{
			var ray = Entity.Eyes.Ray();
			var entity = ray.Run<IPushable>() ?? ray.Radius( size ).Run<IPushable>();
			entity?.OnPush( mouseDelta, Entity );
		}
	}

	public interface IPushable
	{
		void OnPush( float delta, Pawn pawn );
	}
}
