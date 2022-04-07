using System.IO;

namespace Espionage.Engine
{
	public class Depot : IFile
	{
		public Library ClassInfo => Library.Database[typeof( Depot )];
		public FileInfo Info { get; set; }

		public void Save() { }
		public void Load() { }

		public readonly struct Header
		{
			internal Header( BinaryReader reader )
			{
				Indent = reader.ReadBytes( 4 );
				Version = reader.ReadInt32();
				Count = reader.ReadInt32();

				Nodes = new Node[Count];

				for ( var i = 0; i < Count; i++ )
				{
					Nodes[i] = new( reader );
				}
			}

			public byte[] Indent { get; }
			public int Version { get; }
			public int Count { get; }
			public Node[] Nodes { get; }
		}

		public struct Node
		{
			internal Node( BinaryReader reader )
			{
				ClassID = reader.ReadInt32();
				EntityID = reader.ReadBytes( 16 );
				Start = reader.ReadInt32();
				End = reader.ReadInt32();
			}

			public int ClassID { get; }
			public byte[] EntityID { get; }
			public int Start { get; }
			public int End { get; }
		}
	}

	public ref struct Save
	{
		private byte[] EntityID { get; }
		private int ClassID { get; }
		private readonly int Length;

		public Save( Entity entity )
		{
			EntityID = null;
			ClassID = 0;
			Length = 0;
		}

		internal Depot.Node Finish()
		{
			return default;
		}
	}

	public class Restore
	{
		public BinaryReader Reader { get; }
		public Depot.Node Scope { get; }

		public Restore( BinaryReader reader, Depot.Node node )
		{
			Reader = reader;
			Scope = node;
		}
	}
}
