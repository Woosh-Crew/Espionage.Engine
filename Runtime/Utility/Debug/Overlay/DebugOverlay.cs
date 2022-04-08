using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
namespace Espionage.Engine
{	
	/// <summary>A class which will draw debug shapes in the world</summary>
    public static class Overlay
    {
		/// <summary>Returns a material of the overlay wireframe shader</summary>
		public static Material WireframeOverlay{
			get{
				return new Material(Shader.Find("Unlit/DebugOverlay"));
			}
		}

		/// <summary>Returns a material of the geometry wireframe shader</summary>
		public static Material WireframeGeo{
			get{
				return new Material(Shader.Find("Unlit/DebugGeo"));
			}
		}
	
		public static void DrawSphere(Vector3 position, float radius,float time = 0f, Color? color = null, bool depthTest = false){
			//Allows us to have a default color
			if(color == null){
				color = Color.red;
			}
			
			//Pick betwen the overlay or geomtry shader
			Material mat = (!depthTest) ? WireframeOverlay : WireframeGeo;
			mat.SetColor("_Color",color.Value);

			//Get the sphere primitive
			Mesh mesh = PrimitiveFactory.GetPrimitiveMesh(PrimitiveType.Sphere);
			
			//Draw the msh
			Draw(position,new Vector3(radius,radius,radius),time,mesh,mat);
		}

		public static void DrawBox(Vector3 position, Vector3 size,float time = 0f, Color? color = null, bool depthTest = false){
			//Allows us to have a default color
			if(color == null){
				color = Color.red;
			}
			
			//Pick betwen the overlay or geomtry shader
			Material mat = (!depthTest) ? WireframeOverlay : WireframeGeo;
			mat.SetColor("_Color",color.Value);

			//Get the sphere primitive
			Mesh mesh = PrimitiveFactory.GetPrimitiveMesh(PrimitiveType.Cube);
			
			//Draw the msh
			Draw(position,size,time,mesh,mat);
		}

		public static void DrawCapsule(Vector3 position, Vector3 size,float time = 0f, Color? color = null, bool depthTest = false){
			//Allows us to have a default color
			if(color == null){
				color = Color.red;
			}
			
			//Pick betwen the overlay or geomtry shader
			Material mat = (!depthTest) ? WireframeOverlay : WireframeGeo;
			mat.SetColor("_Color",color.Value);

			//Get the sphere primitive
			Mesh mesh = PrimitiveFactory.GetPrimitiveMesh(PrimitiveType.Capsule);
			
			//Draw the msh
			Draw(position,size,time,mesh,mat);
		}

		public static void DrawMesh(Mesh mesh,Vector3 position, Vector3 size,float time = 0f, Color? color = null, bool depthTest = false){
			//Allows us to have a default color
			if(color == null){
				color = Color.red;
			}
			
			//Pick betwen the overlay or geomtry shader
			Material mat = (!depthTest) ? WireframeOverlay : WireframeGeo;
			mat.SetColor("_Color",color.Value);
			
			//Draw the msh
			Draw(position,size,time,mesh,mat);
		}
		
		/// <summary>DrawDraws a given mesh with a material at a position with a scale</summary>
		private static async void Draw(Vector3 position, Vector3 scale, float seconds, Mesh mesh,Material mat){
			//Calculate a transform matrix for a given position, rotation, and scale
			Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, scale);
			
			//Draw mesh draws for one frame, so if seconds is 0f, just draw it once
			if(seconds <= 0f){
				Graphics.DrawMesh(mesh, matrix, mat, 0);
			}

			//Otherwise draw the mesh for X number of seconds
			for(float i = 0; i < seconds; i += Time.deltaTime){
				Graphics.DrawMesh(mesh, matrix, mat, 0);
				await Task.Yield();
			}
		}
    }

	/// <summary>This is a helper class which helps create and cache static references to unity Primitive meshes</summary>
	/// TODO: Worth looking into just doing these as manual triangulation, but this allows us to use unity primitives.
	public static class PrimitiveFactory
	{
		/// <summary>Holds a cache of meshes that we've already requested to avoid overhead of having to create all the objects</summary>
	    private static Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();

	    public static Mesh GetPrimitiveMesh(PrimitiveType type)
	    {
			//If we don't have the primitive cached, create and cache it.
	        if (!PrimitiveFactory.primitiveMeshes.ContainsKey(type))
	        {
	            PrimitiveFactory.CreatePrimitiveMesh(type);
	        }

			//Return the given mesh
	        return PrimitiveFactory.primitiveMeshes[type];
	    }

		/// <summary>Creates a mesh of a given primitive type, stores it into the di</summary>
	    private static Mesh CreatePrimitiveMesh(PrimitiveType type)
	    {
			//Create the gameoject as one of the primitives
	        GameObject gameObject = GameObject.CreatePrimitive(type);
			//Store a reference to the mesh
	        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
			//Destroy the object
	        GameObject.Destroy(gameObject);

			//Cash it into the dictionary
	        PrimitiveFactory.primitiveMeshes[type] = mesh;
	        return mesh;
	    }
	}
}
