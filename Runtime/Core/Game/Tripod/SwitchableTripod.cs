using System.Linq;

namespace Espionage.Engine.Tripods
{
	public class SwitchableTripod : Component<Pawn>, Pawn.ICallbacks, ISimulated
	{
		private Tripod[] Tripods { get; set; }
		public Binding Binding { get; set; }

		protected override void OnAttached( Pawn pawn )
		{
			Tripods = Entity.Components.GetAll<Tripod>().ToArray();
		}

		public void Possess( Client client )
		{
			Entity.Tripod = Tripods[_index];
		}

		private int _index = 0;

		public void Simulate( Client cl )
		{
			Binding ??= Controls.Scheme["Switch.Tripod"];
			
			if ( Binding.Pressed )
			{
				_index++;
				_index %= Tripods.Length;
				Entity.Tripod = Tripods[_index];
			}
		}
	}
}
