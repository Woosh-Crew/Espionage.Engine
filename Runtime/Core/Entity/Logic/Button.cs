namespace Espionage.Engine.Entities
{
	public class Button : Entity, IUsable
	{
		public void OnInteract( Pawn user ) { }
		
		public bool CanUse( Pawn user )
		{
			return true;
		}
	}
}
