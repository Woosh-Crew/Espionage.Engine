namespace Espionage.Engine.Resources
{
	[Group( "Files" )]
	public interface IFile<T, out TOutput> : IFile<T> where T : IResource
	{
		Resource.IProvider<T, TOutput> Provider();
	}

	[Group( "Files" )]
	public interface IFile<T> : IFile where T : IResource { }
}
