using UnityEngine;

namespace Espionage.Engine
{
	public static class Primitives
	{
		private static Mesh[] Meshes { get; } = new Mesh[System.Enum.GetValues( typeof( PrimitiveType ) ).Length];

		public static Mesh GetMesh( PrimitiveType type )
		{
			return Meshes[(int)type] ??= CreatePrimitiveMesh( type );
		}

		private static Mesh CreatePrimitiveMesh( PrimitiveType type )
		{
			var gameObject = GameObject.CreatePrimitive( type );
			var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			Object.Destroy( gameObject );
			return mesh;
		}
	}
}
