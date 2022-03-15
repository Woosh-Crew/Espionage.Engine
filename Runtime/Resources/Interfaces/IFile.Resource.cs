namespace Espionage.Engine.Resources
{

	[Group( "Files" )]
	public interface IFile<T, out TOut> : IFile where T : IResource
	{
		Resource.IProvider<T, TOut> Provider { get; }
	}
}
