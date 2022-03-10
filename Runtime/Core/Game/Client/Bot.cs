namespace Espionage.Engine
{
	public class Bot : Client
	{
		private Client Copying { get; set; }

		internal override void Simulate()
		{
			// Copy Input
			if ( Copying != null )
			{
				Input = Copying.Input;
			}

			base.Simulate();
		}
	}
}
