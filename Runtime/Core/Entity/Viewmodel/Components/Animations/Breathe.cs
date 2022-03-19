using Espionage.Engine;
using UnityEngine;

namespace Espionage.Viewmodels
{
    public class Breathe : Viewmodel.Modifier
    {
	    private float _breatheBobDelta;
	    
	    public override void PostCameraSetup( ref Tripod.Setup setup )
	    {
		    var vec = new Vector3();
		    
		    _breatheBobDelta += Time.deltaTime * 0.5f;

		    // Waves
		    vec.x = Mathf.Sin( _breatheBobDelta * 0.5f ) * 0.8f;
		    vec.y = Mathf.Cos( _breatheBobDelta * 1f ) * 1.3f;
		    vec.z = Mathf.Cos( _breatheBobDelta * 0.5f ) * 0.5f;

		    transform.rotation *= Quaternion.Euler( vec.y, vec.x, vec.z );

		    transform.position += transform.rotation * Vector3.up * vec.y * 0.01f;
		    transform.position += transform.rotation * Vector3.left * vec.x * 0.01f;
	    }
    }
}
