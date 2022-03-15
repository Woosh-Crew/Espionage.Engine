namespace Espionage.Engine.Resources
{

	[Group( "Files" )]
	public interface IFile<T> : IFile where T : IResource
	{
		IBinder<T> Binder { get; }
	}
}
