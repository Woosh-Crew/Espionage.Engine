using System;
using System.IO;
using System.Text;

namespace Espionage.Engine
{
	public class Depot : IFile
	{
		public Library ClassInfo => Library.Database[typeof( Depot )];
		public FileInfo Info { get; set; }

		// Data

		public Header Head { get; private set; }

		// API

		public void Save()
		{
			using var stream = new MemoryStream();
			using var writer = new BinaryWriter( stream );

			// Create Header
			var header = new Header();
			header.Serialize( writer );
			// Save each entities data

			// Save to file.
		}

		public void Load()
		{
			// Read Header
			using var stream = Info.Open( FileMode.Open, FileAccess.Read );
			using var reader = new BinaryReader( stream );

			Head = new( reader );
		}

		// 12 + ( 28 * Count )
		public readonly struct Header
		{
			public string Format => Encoding.UTF8.GetString( Indent );

			internal Header( BinaryReader reader )
			{
				Indent = reader.ReadBytes( 4 );

				// Unity Save Format Validator 
				if ( Encoding.UTF8.GetString( Indent ) != "USVE" )
				{
					Count = 0;
					Version = 0;
					Nodes = null;

					Debugging.Log.Error( "Invalid File" );
					return;
				}

				Version = reader.ReadInt32();
				Count = reader.ReadInt32();

				Nodes = new Node[Count];

				for ( var i = 0; i < Count; i++ )
				{
					Nodes[i] = new( reader );
				}
			}

			public void Serialize( BinaryWriter writer )
			{
				writer.Write( new char[]
				{
					'U',
					'S',
					'V',
					'E'
				} );

				writer.Write( 150 );
			}

			public byte[] Indent { get; }
			public int Version { get; }
			public int Count { get; }
			public Node[] Nodes { get; }
		}

		// 28 Bytes
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
			public long Start { get; }
			public long End { get; }
		}
	}

	public ref struct Save
	{
		public BinaryWriter Writer { get; }
		
		public int ClassID { get; }
		public byte[] EntityID { get; }
		public long Start { get; }
		public long End { get; }

		public Save( Entity entity ,BinaryWriter writer )
		{
			Writer = writer;
			
			ClassID = entity.ClassInfo.Id;
			EntityID = entity.UniqueID.ToByteArray();

			Start = writer.BaseStream.Position;
			End = 0;
		}
		
		public void Write( int value ) { }

		internal Depot.Node Finish() { return default; }
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
