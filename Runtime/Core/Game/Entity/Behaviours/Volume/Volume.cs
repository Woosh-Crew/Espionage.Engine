using System;
using UnityEngine;

namespace Espionage.Engine.Volumes
{
	public sealed class Volume : Entity
	{
		public interface ICallbacks
		{
			void OnEnter( Entity entity );
			void OnExit( Entity entity );

			/// <param name="distance">
			/// Normalised distance from the blend distance to how far away
			/// you are from the volume.
			/// </param>
			void OnStay( float distance );
		}

		private void OnTriggerEnter( Collider other ) { }

		private void OnTriggerExit( Collider other ) { }

		private void OnTriggerStay( Collider other ) { }


		// Fields

		[SerializeField]
		private float blendDistance;

		[SerializeField]
		private int priority;
	}
}
