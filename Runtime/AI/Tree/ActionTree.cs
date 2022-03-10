using System.IO;
using Espionage.Engine.Resources;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), File( Extension = "uait" )]
	public sealed class ActionTree : IFile<AIGraph>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( ActionTree )];

		public FileInfo File { get; set; }

		public void Load( FileStream fileStream ) { }
	}
}
