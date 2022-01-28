namespace Espionage.Engine.Components
{
	public interface IComponent { }

	public interface IComponent<in T> : IComponent where T : class
	{
		void OnAttached( T item );
		void OnDetached( T item ) { }
	}
}
