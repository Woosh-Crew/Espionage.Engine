namespace Espionage.Engine
{
	public class Controller
	{
		/// <summary> Called when Controller should think, usually called every frame </summary>
		/// <param name="delta"> The time between last think </param>
		public virtual void Think( float delta ) { }
	}
}
