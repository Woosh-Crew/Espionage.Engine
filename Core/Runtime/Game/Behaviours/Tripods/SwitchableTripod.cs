using UnityEngine;

namespace Espionage.Engine.Tripods
{
	public class SwitchableTripod : Component<Pawn>, ISimulated
	{
		private int _index;

		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );
			Entity.Tripod = tripods[0];
		}

		public void Simulate( Client cl )
		{
			if ( Input.GetKeyDown( KeyCode.C ) )
			{
				_index++;
				_index %= tripods.Length ;
				Entity.Tripod = tripods[_index];
			}
		}
		
		// Fields

		[SerializeField]
		private Tripod[] tripods;
	}
}
