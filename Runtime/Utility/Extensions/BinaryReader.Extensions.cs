using System.IO;
using UnityEngine;

namespace Espionage.Engine
{
	public static class BinaryReaderExtensions
	{
		public static Vector3 ReadVec3( this BinaryReader reader )
		{
			var x = reader.ReadSingle();
			var y = reader.ReadSingle();
			var z = reader.ReadSingle();

			return new( x, y, z );
		}

		public static Vector3 ReadSourceVec3( this BinaryReader reader )
		{
			var x = reader.ReadSingle();
			var z = reader.ReadSingle();
			var y = reader.ReadSingle();

			return new( x, y, z );
		}
	}
}
