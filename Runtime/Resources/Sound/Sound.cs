using System.IO;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Group( "Sounds" ), Path( "sounds", "assets://Sounds" ), File( Extension = "usnd" )]
	public class Sound : Data
	{
		public bool UI { get; set; }
		public float Distance { get; set; }

		public Vector2 Volume { get; set; }
		public Vector2 Pitch { get; set; }

		public string[] Sounds { get; set; }

		// Asset Compiling & Loading
		// --------------------------------------------------------------------------------------- //

		protected override void OnCompile( BinaryWriter writer )
		{
			// Generic

			writer.Write( UI );
			writer.Write( Distance );

			// Volume

			writer.Write( Volume.x );
			writer.Write( Volume.y );

			// Pitch

			writer.Write( Pitch.x );
			writer.Write( Pitch.y );

			// Sounds

			writer.Write( Sounds.Length );

			for ( var i = 0; i < Sounds.Length; i++ )
			{
				writer.Write( Sounds[i] );
			}
		}

		protected override void OnLoad( BinaryReader reader )
		{
			UI = reader.ReadBoolean();
			Distance = reader.ReadSingle();
			Volume = new( reader.ReadSingle(), reader.ReadSingle() );
			Pitch = new( reader.ReadSingle(), reader.ReadSingle() );

			Sounds = new string[reader.ReadInt32()];

			for ( var i = 0; i < Sounds.Length; i++ )
			{
				Sounds[i] = reader.ReadString();
			}
		}
	}
}
