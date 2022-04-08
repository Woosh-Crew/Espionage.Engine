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

		public void Save() { }

		public void Load()
		{
			// Read Header
			using var stream = Info.Open( FileMode.Open, FileAccess.Read );
			using var reader = new BinaryReader( stream );

			Head = new( reader );
		}

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

					Dev.Log.Error( "Invalid File" );
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
		public void WriteInt32( int value ) { }
		public void WriteFloat( float value ) { }
		public void WriteBool( bool value ) { }

		internal byte[] Finish()
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
