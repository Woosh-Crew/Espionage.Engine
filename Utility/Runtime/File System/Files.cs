using System.Diagnostics.Tracing;
using System.IO;
using System.Text;

namespace Espionage.Engine
{
	public static class Files
	{
		// Just Give us the Raw Data.
		private static byte[] Load( string path )
		{
			if ( !File.Exists( path ) )
			{
				throw new FileNotFoundException( "File doesn't Exist" );
			}

			return File.ReadAllBytes( path );
		}

		private static void Save( string text, string path )
		{
			Save( Encoding.UTF8.GetBytes( text ), path );
		}

		private static void Save( byte[] data, string path )
		{
			if ( !Directory.Exists( path ) )
			{
				Directory.CreateDirectory( path );
			}

			using var stream = File.Create( path );
			stream.Write( data );
		}


		private static FileStream Read( string path )
		{
			return new FileStream( path, FileMode.Open, FileAccess.Read );
		}
	}
}
