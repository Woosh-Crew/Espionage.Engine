using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
namespace Espionage.Engine
{
	/// <summary>This class will draw some shit</summary>
    public static class Overlay
    {
		//Note for Jake:
		//It's a little scuffed and is gonna be cleaned up
		

		public static void DrawSphere(Vector3 position, float radius,float time = 0f, Color? color = null){
			//Weird hack for optional color
			if(color == null){
				color = Color.green;
			}
			
			//Weird hack for getting materials
			//TODO: There's gotta be a more performant way to do this, or maybe this isn't super expensive, who knows.
			Material mat = new Material(Shader.Find("Unlit/DebugOverlay"));
			mat.SetColor("_Color",color.Value);
			Mesh mesh = PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Sphere);
			
			Draw(time,mesh,mat,position,radius);
			
			

		}

		public static async void Draw(float seconds, Mesh mesh,Material mat, Vector3 position, float scale){
			Vector3 size = new Vector3(scale,scale,scale);
			Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, size);
			
			//Draw for a single frame
			if(seconds <= 0f){
				Graphics.DrawMesh(mesh, matrix, mat, 0);
			}

			//Fun little method for doing something 'x' number of seconds
			for(float i = 0; i < seconds; i += Time.deltaTime){
				Graphics.DrawMesh(mesh, matrix, mat, 0);
				await Task.Yield();
			}
		}
    }

	/// <summary>This is a helper class which helps create and cache static references to unity Primitive meshes</summary>
	/// This was wonderfully provided in an answer to getting static mesh primitives on the unity forums
	/// https://answers.unity.com/questions/514293/changing-a-gameobjects-primitive-mesh.html
	/// In theory we could just triangulate our own primitives but thats a task for another day
	public static class PrimitiveHelper
	{
	    private static Dictionary<PrimitiveType, Mesh> primitiveMeshes = new Dictionary<PrimitiveType, Mesh>();
	
	    public static GameObject CreatePrimitive(PrimitiveType type, bool withCollider)
	    {
	        if (withCollider) { return GameObject.CreatePrimitive(type); }
	
	        GameObject gameObject = new GameObject(type.ToString());
	        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
	        meshFilter.sharedMesh = PrimitiveHelper.GetPrimitiveMesh(type);
	        gameObject.AddComponent<MeshRenderer>();
	
	        return gameObject;
	    }
	
	    public static Mesh GetPrimitiveMesh(PrimitiveType type)
	    {
	        if (!PrimitiveHelper.primitiveMeshes.ContainsKey(type))
	        {
	            PrimitiveHelper.CreatePrimitiveMesh(type);
	        }
	
	        return PrimitiveHelper.primitiveMeshes[type];
	    }
	
	    private static Mesh CreatePrimitiveMesh(PrimitiveType type)
	    {
	        GameObject gameObject = GameObject.CreatePrimitive(type);
	        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
	        GameObject.Destroy(gameObject);
	
	        PrimitiveHelper.primitiveMeshes[type] = mesh;
	        return mesh;
	    }
	}
}
