namespace Espionage.Engine
{
	public interface IUsable
	{
		/// <returns>
		/// Return true if we're done with it. Return false
		/// if we should simulate it. (E.g.) HL2 Energy and
		/// Health rechargers, or valves...
		/// </returns>
		bool OnUse( Pawn user );

		void Started( Pawn user ) { }
		void Stopped( Pawn user ) { }

		bool IsUsable( Pawn user );
	}
}
