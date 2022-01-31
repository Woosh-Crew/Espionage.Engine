namespace Espionage.Engine.Components
{
	public interface IComponent<in T> where T : class
	{
		void OnAttached( T item );
		void OnDetached( T item ) { }
	}
}
