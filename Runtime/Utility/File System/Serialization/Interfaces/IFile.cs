using System.IO;

namespace Espionage.Engine
{
	[Group( "Files" )]
	public interface IFile : ILibrary
	{
		FileInfo Info { get; set; }
	}
}
