using UnityEngine;

namespace Espionage.Engine.Viewmodels
{
	public sealed class Guntuck : Viewmodel.Effect
	{
		private float _lastGuntuckOffset;

		public override void PostCameraSetup( ref ITripod.Setup setup )
		{
			// Get girth
			float girth = 1;

			// Start Guntuck
			var start = muzzle.position + muzzle.rotation * Vector3.back * Vector3.Distance( transform.position - muzzle.rotation * Vector3.back / 4, muzzle.position ) - muzzle.rotation * Vector3.back * girth / 2.25f;
			var end = muzzle.position + muzzle.rotation * Vector3.forward;

			Physics.Raycast( start, end, out var tr, 2 );

			var offset = tr.distance - Vector3.Distance( start, end );
			_lastGuntuckOffset = Mathf.Lerp( _lastGuntuckOffset, offset, 8 * Time.deltaTime );

			Debug.DrawLine( start, end, Color.blue );

			// Finish Guntuck
			transform.position += transform.rotation * Vector3.back * -_lastGuntuckOffset;
			transform.position += transform.rotation * Vector3.down * -_lastGuntuckOffset / 4;
		}

		// Fields

		[SerializeField]
		private Transform muzzle;
	}
}
