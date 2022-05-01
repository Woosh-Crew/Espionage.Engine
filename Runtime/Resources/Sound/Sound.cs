using System.IO;

namespace Espionage.Engine.Resources
{
	[Group( "Sounds" ), Path( "sounds", "assets://Sounds" ), File( Extension = "usnd" )]
	public class Sound : Data
	{
		// Asset Compiling & Loading
		// --------------------------------------------------------------------------------------- //
		
		protected override void OnCompile( BinaryWriter writer )
		{
			base.OnCompile( writer );
		}

		protected override void OnLoad( BinaryReader reader )
		{
			base.OnLoad( reader );
		}
	}
}
