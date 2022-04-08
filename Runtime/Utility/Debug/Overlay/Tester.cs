using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
    public class Tester : MonoBehaviour
    {
    public void Update() {

		Overlay.DrawSphere(transform.position,2f,0f,Color.blue);
    }
    }
}
