using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary> This is a helper class which helps create and cache static references to unity Primitive meshes </summary>
	public static class Primitives
	{
		/// <summary> Holds a cache of meshes that we've already requested to avoid overhead of having to create all the objects </summary>
		private static Dictionary<PrimitiveType, Mesh> Meshes { get; } = new();
		
		public static Mesh GetMesh( PrimitiveType type )
		{
			//If we don't have the primitive cached, create and cache it.
			if ( !Meshes.ContainsKey( type ) )
			{
				CreatePrimitiveMesh( type );
			}

			//Return the given mesh
			return Meshes[type];
		}

		/// <summary> Creates a mesh of a given primitive type, stores it into the database </summary>
		private static Mesh CreatePrimitiveMesh( PrimitiveType type )
		{
			// Create the GameObject as one of the primitives
			var gameObject = GameObject.CreatePrimitive( type );
			//Store a reference to the mesh
			var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			//Destroy the object
			Object.Destroy( gameObject );

			//Cash it into the dictionary
			Meshes[type] = mesh;
			return mesh;
		}
	}
}
