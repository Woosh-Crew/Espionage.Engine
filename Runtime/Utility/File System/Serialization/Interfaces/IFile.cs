using System.IO;

namespace Espionage.Engine
{
	[Group( "Files" )]
	public interface IFile : ILibrary
	{
		FileInfo Source { get; set; }
		void Load( FileStream fileStream );
	}
}
