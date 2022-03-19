using Espionage.Engine;
using UnityEngine;

namespace Espionage
{
    public class SourceSway : Viewmodel.Modifier
    {
	    private Vector2 _lastMouseDelta;
	    private Vector3 _lastPosition;
	    
	    public override void PostCameraSetup( ref Tripod.Setup setup )
	    {
		    _lastMouseDelta += Controls.Mouse.Delta * Time.deltaTime;
		    _lastMouseDelta = _lastMouseDelta.LerpTo( Vector2.zero, smoothing * Time.deltaTime );

		    var vec = new Vector3();
		    vec.x = -_lastMouseDelta.x;
		    vec.y = -_lastMouseDelta.y;
		    vec.z = 0;

		    _lastPosition = _lastPosition.LerpTo( vec, 6 * Time.deltaTime );
		    transform.position +=  _lastPosition * intensity ;
	    }
	    
	    // Fields
	    
	    [SerializeField]
	    private float smoothing = 10;
	    	    
	    [SerializeField]
	    private float intensity = 0.1f;
    }
}
