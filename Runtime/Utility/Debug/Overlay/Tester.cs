using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
    public class Tester : MonoBehaviour
    {
		public Mesh mesh;
    public Material material;
    public void Update() {
		Debug.Log("HMM");
        // will make the mesh appear in the scene at origin position
        //Graphics.DrawMesh(mesh, transform.position, Quaternion.identity, material, 0);
		Overlay.DrawSphere(transform.position,2f,1f,Color.blue);
    }
    }
}
