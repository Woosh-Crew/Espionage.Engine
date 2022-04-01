namespace Espionage.Engine
{
	[Group( "Pickups" )]
	public class Pickup : Entity
	{
		/// <summary>The actor holding this pickup</summary>
		public Actor Carrier { get; private set; }

		/// <summary>The Stats for a given weapon</summary>
		public PickupData Stats;

		/// <summary>If this item can be dropped</summary>
		public virtual bool Droppable => true;

		/// <summary>Called when item picked up</summary>
		public virtual void OnPickup( Actor carrier )
		{
			Carrier = carrier;
		}

		/// <summary>Called when item dropped</summary>
		public virtual void OnDrop( Actor dropper )
		{
			Carrier = null;
		}

	}
	
}
