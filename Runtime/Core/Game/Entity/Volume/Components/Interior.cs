namespace Espionage.Engine.Volumes
{
	[Help( "Interiors are used for defining spaces where wind and other outside forces can't go" )]
	public class Interior : MonoComponent<Volume>, Volume.ICallbacks
	{
		public void OnEnter( Entity entity ) { }

		public void OnStay( float distance ) { }

		public void OnExit( Entity entity ) { }
	}
}
