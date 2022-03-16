using System.IO;
using Espionage.Engine.Resources.Binders;

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Portable Network Graphics" ), File( Extension = "png" )]
	public class PNG : Texture.File
	{
		public override Texture.Binder Binder => _binder;

		private GenericTextureBinder _binder;

		public override void Load( FileStream fileStream )
		{
			using var reader = new BinaryReader( fileStream );

			var buffer = new byte[ fileStream.Length];
			fileStream.Read( buffer );
			
			_binder = new ( Source.FullName, buffer );
		}
	}
}
