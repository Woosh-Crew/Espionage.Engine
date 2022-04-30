using UnityEngine;

namespace Espionage.Engine
{
	public  partial class Entity
	{
		private class Hook : MonoBehaviour
		{
			internal Entity Owner { get; set; }

			private void OnDestroy()
			{
				Owner?.Delete();
				Owner = null;
			}

			private Entity GetEntity()
			{
				return Owner;
			}

			// Unity Events

			private void OnCollisionEnter( Collision collision )
			{
				GetEntity()?.OnCollisionEnter( collision );
			}

			private void OnCollisionExit( Collision other )
			{
				GetEntity()?.OnCollisionExit( other );
			}

			private void OnCollisionStay( Collision collisionInfo )
			{
				GetEntity()?.OnCollisionStay( collisionInfo );
			}

			private void OnTriggerEnter( Collider other )
			{
				GetEntity()?.OnTriggerEnter( other );
			}

			private void OnTriggerExit( Collider other )
			{
				GetEntity()?.OnTriggerExit( other );
			}

			private void OnTriggerStay( Collider other )
			{
				GetEntity()?.OnTriggerStay( other );
			}
		}
	}
}
