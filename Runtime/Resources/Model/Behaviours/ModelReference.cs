namespace Espionage.Engine.Resources.Internal
{
	public class ModelReference : Behaviour
	{
		public Model Model { get; internal set; }

		protected override void OnDelete()
		{
			base.OnDelete();
			
			Model?.Remove( gameObject );
		}
	}
}
