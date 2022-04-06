namespace Espionage.Engine.Components
{
	public interface IComponent<in T> : IComponent where T : class
	{
		bool CanAttach( T item ) { return true; }

		void OnAttached( T item );
		void OnDetached() { }
	}

	public interface IComponent { }
}
