using System.IO;

namespace Espionage.Engine.Resources
{
	[Group( "Surfaces" ), Path( "surface", "data://Surfaces" ), File( Extension = "surf" )]
	public sealed class Surface : Data
	{
		public float Friction { get; set; }
		public float Density { get; set; }
		public string[] Footsteps { get; set; }
		public string[] Impacts { get; set; }

		// Asset Compiling & Loading
		// --------------------------------------------------------------------------------------- //

		protected override void OnCompile( BinaryWriter writer )
		{
			writer.Write( Friction );
			writer.Write( Density );

			// Footsteps

			writer.Write( Footsteps.Length );

			for ( var i = 0; i < Footsteps.Length; i++ )
			{
				writer.Write( Footsteps[i] );
			}

			// Impacts

			writer.Write( Impacts.Length );

			for ( var i = 0; i < Impacts.Length; i++ )
			{
				writer.Write( Impacts[i] );
			}
		}

		protected override void OnLoad( BinaryReader reader )
		{
			Friction = reader.ReadSingle();
			Density = reader.ReadSingle();

			// Footsteps

			Footsteps = new string[reader.ReadInt32()];

			for ( var i = 0; i < Footsteps.Length; i++ )
			{
				Footsteps[i] = reader.ReadString();
			}

			// Impacts

			Impacts = new string[reader.ReadInt32()];

			for ( var i = 0; i < Impacts.Length; i++ )
			{
				Impacts[i] = reader.ReadString();
			}
		}
	}

	public interface ISurface
	{
		Surface Surface { get; }
	}
}
