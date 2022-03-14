namespace Espionage.Engine.Resources
{

	[Group( "Files" )]
	public interface IFile<T> : IFile where T : IResource
	{
		Resource.IProvider<T> Provider { get; }
	}
}
