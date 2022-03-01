namespace Espionage.Engine.Resources
{
	public interface IFile<T, out TOutput> : IFile where T : Resource
	{
		Resource.IProvider<T, TOutput> Provider();
	}
}
