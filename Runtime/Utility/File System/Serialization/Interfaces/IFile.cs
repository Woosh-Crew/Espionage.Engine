using System.IO;

namespace Espionage.Engine
{
	[Group( "Files" )]
	public interface IFile : ILibrary
	{
		FileInfo File { get; set; }
		void Load( FileStream fileStream );
	}
}
