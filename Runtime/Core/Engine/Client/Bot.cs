namespace Espionage.Engine
{
	public class Bot : Client
	{
		internal Bot( string name ) : base( name ) { }
		
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
