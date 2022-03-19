using Espionage.Engine;
using UnityEngine;

namespace Espionage.Viewmodels
{
    public class VelocityOffset : Viewmodel.Modifier
    {
	    private Vector3 _dampedOffset;
	    
	    public override void PostCameraSetup( ref Tripod.Setup setup )
	    {
		    var offset = Local.Pawn.Velocity.WithY(0);
		    _dampedOffset = _dampedOffset.LerpTo( offset, 6 * Time.deltaTime );
		    
		    transform.position -= _dampedOffset * 1.1f;
	    }
    }
}
