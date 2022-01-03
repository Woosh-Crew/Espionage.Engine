using System.Threading.Tasks;

namespace Espionage.Engine.Internal.Callbacks
{
	public interface ICallbackProvider
	{
		Task Initialize() { return null; }

		void Run( string name, params object[] args );
		void Register( object item );
		void Unregister( object item );
	}
}
