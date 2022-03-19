namespace Espionage.Engine
{
	public interface IUsable
	{
		void OnInteract( Pawn user );
		bool CanUse( Pawn user );
	}
}
