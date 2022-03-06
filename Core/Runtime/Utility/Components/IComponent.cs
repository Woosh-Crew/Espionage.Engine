namespace Espionage.Engine.Components
{
	public interface IComponent<in T> where T : class
	{
		bool CanAttach( T item ) { return true; }

		void OnAttached( T item );
		void OnDetached( T item ) { }
	}
}
