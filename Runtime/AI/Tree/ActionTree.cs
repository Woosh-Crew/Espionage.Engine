using System.IO;
using Espionage.Engine.Resources;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), File( Extension = "uai" )]
	public sealed class ActionTree : IFile<Graph>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( ActionTree )];

		// Compiling

		public void Compile() { }

		// Deserialization

		public FileInfo File { get; set; }

		public void Load( FileStream fileStream ) { }
	}
}
