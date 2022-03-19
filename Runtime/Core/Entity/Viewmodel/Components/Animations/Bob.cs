using Espionage.Engine;
using UnityEngine;

namespace Espionage.Viewmodels
{
    public class Bob : Viewmodel.Modifier
    {
	    private float _dampedSpeed;
	    private float _walkBobDelta;
	    
	    public override void PostCameraSetup( ref Tripod.Setup setup )
	    {
		    CalcRandom();
		    
		    // DON"t FUCKING TOUCH THIS STUPID LINE, I WENT  INSANE TRYING TO WORK 
			// OUT WHY IT WAS FRAMERATE DEPENDENT AND FOR SOME DUMB ASS REASON THIS FIXES IT.		    
		    var speed = Local.Pawn.Velocity.WithY( 0 ).magnitude / 6.75f / Time.deltaTime;

		    _dampedSpeed = _dampedSpeed.LerpTo( speed, 2 * Time.deltaTime );
		    var rot = new Vector3();
		    
		    _walkBobDelta += Time.deltaTime * 12.0f * speed;

		    // Waves
		    rot.x = Mathf.Sin( _walkBobDelta * 0.7f ) * 0.6f * _random.x;
		    rot.y = Mathf.Cos( _walkBobDelta * 0.7f ) * 0.4f * _random.y;
		    rot.z = Mathf.Cos( _walkBobDelta * 1.3f ) * 0.8f * _random.z;

		    rot *= _dampedSpeed;

		    transform.rotation *= Quaternion.Euler(  rot.z * 2, rot.y * 4, rot.x * 4  );

		    transform.position += transform.rotation * Vector3.up * rot.z / 40;
		    transform.position += transform.rotation * Vector3.left * rot.y * 1.25f / 40;
	    }
	    
	    // Interpolated Randomness

	    private Vector3 _random;

	    private void CalcRandom()
	    {
		    _random.x = _random.x.LerpTo( Random.Range( 0.6f, 1.2f ), 5 * Time.deltaTime );
		    _random.y = _random.y.LerpTo( Random.Range( 0.7f, 1.4f ), 5 * Time.deltaTime );
		    _random.z = _random.z.LerpTo( Random.Range( 0.5f, 1.1f ), 5 * Time.deltaTime );
	    }
    }
}
