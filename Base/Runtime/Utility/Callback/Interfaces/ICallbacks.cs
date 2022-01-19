namespace Espionage.Engine
{
	/// <summary> Add this interface so an object can receive callbacks </summary>
	public interface ICallbacks
	{
		bool CanCallback( string callback ) { return true; }
	}
}
