using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Espionage.Engine
{
	/// <summary>A class which will draw debug shapes in the world</summary>
	public static class Overlay
	{
		/// <summary> Returns a material of the overlay wireframe shader </summary>
		public static Material WireframeOverlay => new( Shader.Find( "Unlit/DebugOverlay" ) );

		/// <summary> Returns a material of the geometry wireframe shader </summary>
		public static Material WireframeGeo => new( Shader.Find( "Unlit/DebugGeo" ) );

		public static void Sphere( Vector3 position, float radius, float time = 0f, Color? color = null, bool depthTest = false )
		{
			color ??= Color.red;

			// Pick between the overlay or geo shader
			var mat = !depthTest ? WireframeOverlay : WireframeGeo;
			mat.SetColor( "_Color", color.Value );

			//Get the sphere primitive
			var mesh = PrimitiveFactory.GetPrimitiveMesh( PrimitiveType.Sphere );

			//Draw the msh
			Draw( position, new( radius, radius, radius ), time, mesh, mat );
		}

		public static void Box( Vector3 position, Vector3 size, float time = 0f, Color? color = null, bool depthTest = false )
		{
			color ??= Color.red;

			// Pick between the overlay or geo shader
			var mat = !depthTest ? WireframeOverlay : WireframeGeo;
			mat.SetColor( "_Color", color.Value );

			// Get the sphere primitive
			var mesh = PrimitiveFactory.GetPrimitiveMesh( PrimitiveType.Cube );

			// Draw the msh
			Draw( position, size, time, mesh, mat );
		}

		public static void Capsule( Vector3 position, Vector3 size, float time = 0f, Color? color = null, bool depthTest = false )
		{
			color ??= Color.red;

			// Pick between the overlay or geo shader
			var mat = !depthTest ? WireframeOverlay : WireframeGeo;
			mat.SetColor( "_Color", color.Value );

			//Get the sphere primitive
			var mesh = PrimitiveFactory.GetPrimitiveMesh( PrimitiveType.Capsule );

			//Draw the msh
			Draw( position, size, time, mesh, mat );
		}

		public static void Mesh( Mesh mesh, Vector3 position, Vector3 size, float time = 0f, Color? color = null, bool depthTest = false )
		{
			color ??= Color.red;

			// Pick between the overlay or geo shader
			var mat = !depthTest ? WireframeOverlay : WireframeGeo;
			mat.SetColor( "_Color", color.Value );

			// Draw the msh
			Draw( position, size, time, mesh, mat );
		}

		/// <summary> Draws a given mesh with a material at a position with a scale </summary>
		private static async void Draw( Vector3 position, Vector3 scale, float seconds, Mesh mesh, Material mat )
		{
			// Calculate a transform matrix for a given position, rotation, and scale
			var matrix = Matrix4x4.TRS( position, Quaternion.identity, scale );

			// Draw mesh draws for one frame, so if seconds is 0f, just draw it once
			if ( seconds <= 0f )
			{
				Graphics.DrawMesh( mesh, matrix, mat, 0 );
			}

			// Otherwise draw the mesh for X number of seconds
			for ( float i = 0; i < seconds; i += Time.deltaTime )
			{
				Graphics.DrawMesh( mesh, matrix, mat, 0 );
				await Task.Yield();
			}
		}
	}

	/// <summary> This is a helper class which helps create and cache static references to unity Primitive meshes </summary>
	/// TODO: Worth looking into just doing these as manual triangulation, but this allows us to use unity primitives.
	public static class PrimitiveFactory
	{
		/// <summary> Holds a cache of meshes that we've already requested to avoid overhead of having to create all the objects </summary>
		private static readonly Dictionary<PrimitiveType, Mesh> primitiveMeshes = new();

		public static Mesh GetPrimitiveMesh( PrimitiveType type )
		{
			//If we don't have the primitive cached, create and cache it.
			if ( !primitiveMeshes.ContainsKey( type ) )
			{
				CreatePrimitiveMesh( type );
			}

			//Return the given mesh
			return primitiveMeshes[type];
		}

		/// <summary> Creates a mesh of a given primitive type, stores it into the database </summary>
		private static Mesh CreatePrimitiveMesh( PrimitiveType type )
		{
			// Create the gameoject as one of the primitives
			var gameObject = GameObject.CreatePrimitive( type );
			//Store a reference to the mesh
			var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			//Destroy the object
			Object.Destroy( gameObject );

			//Cash it into the dictionary
			primitiveMeshes[type] = mesh;
			return mesh;
		}
	}
}
