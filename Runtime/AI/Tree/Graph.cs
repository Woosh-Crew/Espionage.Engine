using Espionage.Engine.Resources;

namespace Espionage.Engine.AI
{
	[Group( "AI" ), Path( "ai", "assets://AI/" )]
	public class Graph : Resource
	{
		public static Graph Load( string path )
		{
			path = Files.Pathing.Absolute( path );

			if ( !Files.Pathing.Exists( path ) )
			{
				Debugging.Log.Error( $"We couldn't find [{path}]" );
				return null;
			}

			if ( Database[path] is Graph databaseModel )
			{
				((IResource)databaseModel).Load();
				return databaseModel;
			}

			using var _ = Debugging.Stopwatch( $"Loading AI Graph [{Files.Pathing.Absolute( path )}]" );

			var model = new Graph( Files.Serialization.Load<ActionTree>( path ) );
			((IResource)model).Load();
			return model;
		}

		//
		// Instance
		//

		public override string Identifier { get; }
		public ActionTree Tree { get; }

		protected Graph( ActionTree tree )
		{
			Tree = tree;
			Identifier = tree.File.FullName;
		}
	}
}
