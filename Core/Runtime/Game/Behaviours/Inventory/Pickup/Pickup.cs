namespace Espionage.Engine
{
	public class Pickup : Behaviour
	{
		public Pawn Owner { get; private set; }
		
		public virtual bool Droppable => true;

		public virtual void OnPickup( Pawn carrier )
		{
			Owner = carrier;
		}

		public virtual void OnDrop( Pawn dropper )
		{
			Owner = null;
		}
	}
}
