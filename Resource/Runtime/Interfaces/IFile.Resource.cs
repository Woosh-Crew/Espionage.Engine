namespace Espionage.Engine.Resources
{
	public interface IFile<T, out TOutput> : IFile where T : IResource
	{
		Resource.IProvider<T, TOutput> Provider();
	}
}
