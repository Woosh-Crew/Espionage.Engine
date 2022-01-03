namespace Espionage.Engine.Internal.Callbacks
{
	public interface ICallbackProvider
	{
		void Run( string name );
		void Register( object item );
		void Unregister( object item );
	}
}
