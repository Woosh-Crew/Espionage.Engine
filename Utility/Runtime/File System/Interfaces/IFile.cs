using System.IO;

namespace Espionage.Engine
{
	public interface IFile : ILibrary
	{
		FileInfo File { get; set; }
		void Load( FileStream fileStream );
	}
}
