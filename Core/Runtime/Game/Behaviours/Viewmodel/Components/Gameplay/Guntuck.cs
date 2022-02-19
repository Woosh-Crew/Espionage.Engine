using System.Diagnostics;
using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
    public sealed class Guntuck : Behaviour, Viewmodel.IEffect
    {
	    private float _lastGuntuckOffset;
	    
	    public void PostCameraSetup( ref ITripod.Setup setup )
	    {
		    // Get girth
		    float girth = 1;

		    // Start Guntuck
		    var start = muzzle.Position + muzzle.Rotation.Backward * Vector3.DistanceBetween( Position - muzzle.Rotation.Backward / 4, muzzle.Position ) - (Rotation.Backward * girth / 2.25f);
		    var end = muzzle.Position + (muzzle.Rotation.Forward * 4);

		    var tr = Trace.Ray( start, end )
			    .Ignore( Local.Pawn )
			    .Ignore( Entity )
			    .Size( 1 )
			    .Run();

		    var offset = tr.Distance - Vector3.DistanceBetween( start, end );
		    _lastGuntuckOffset = _lastGuntuckOffset.LerpTo( offset, 8 * Time.Delta );

		    // Finish Guntuck
		    Position += Rotation.Backward * -_lastGuntuckOffset;
		    Position += Rotation.Down * -_lastGuntuckOffset / 4;
	    }
	    
	    // Fields

	    [SerializeField]
	    private Transform muzzle;
    }
}
