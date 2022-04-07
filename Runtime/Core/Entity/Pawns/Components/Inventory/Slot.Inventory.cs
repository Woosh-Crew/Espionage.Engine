namespace Espionage.Engine
{
	public class SlotInventory : Inventory, IHasHoldable
	{
		public Holdable[] Slots { get; private set; } = new Holdable[5];
		public Holdable Active { get; protected set; }
	}
}
