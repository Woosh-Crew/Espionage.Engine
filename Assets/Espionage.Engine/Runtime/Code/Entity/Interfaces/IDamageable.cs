namespace Espionage.Engine
{
	public interface IDamageable
	{
		bool Dead => Health <= 0;
		float Health { get; set; }

		void TakeDamage( float amount )
		{
			Health -= amount;
		}
	}
}
