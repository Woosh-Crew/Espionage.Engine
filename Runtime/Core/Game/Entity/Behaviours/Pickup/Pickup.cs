namespace Espionage.Engine
{
	[Group( "Pickups" )]
	public class Pickup : Entity
	{
		public Actor Carrier { get; private set; }

		public virtual bool Droppable => true;

		public virtual void OnPickup( Actor carrier )
		{
			Carrier = carrier;
		}

		public virtual void OnDrop( Actor dropper )
		{
			Carrier = null;
		}
	}
}
