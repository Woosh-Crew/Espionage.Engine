using Espionage.Engine.Resources;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Path( "ai", "assets://AI/" )]
	public class AIGraph : Resource
	{
		public static AIGraph Load( string path )
		{
			return Database[path] as AIGraph ?? new AIGraph( Files.Load<ActionTree>( path ) );
		}

		//
		// Instance
		//

		public override string Identifier { get; }
		public ActionTree Tree { get; }

		protected AIGraph( ActionTree tree )
		{
			Tree = tree;
			Identifier = tree.File.FullName;
		}
	}
}
