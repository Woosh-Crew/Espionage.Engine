using System;
using UnityEngine;

namespace Espionage.Engine
{
	[SelectionBase, DisallowMultipleComponent]
	public class Proxy : Behaviour
	{
		[Serializable]
		public class Sheet
		{
			public string key;
			public string value;
		}
		
		public Entity Create()
		{
			var ent = Library.Create( className ) as Entity;

			if ( ent != null )
			{
				ent.MoveTo( transform );
				
				ent.Name = name;
				ent.Enabled = !disabled;
			}

			return ent;
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			Gizmos.color = new Color( 0.5f, 1, 1, 0.8f );
			Gizmos.DrawWireCube( Vector3.zero, Vector3.one / 3 );

			Gizmos.color = new Color( 0.5f, 1, 1, 0.5f );
			Gizmos.DrawCube( Vector3.zero, Vector3.one / 3 );

			Gizmos.matrix = Matrix4x4.zero;
			Gizmos.color = Color.white;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;

			Gizmos.color = new Color( 0.2f, 0.5f, 0.5f, 1 );
			Gizmos.DrawWireCube( Vector3.zero, Vector3.one / 3 );

			Gizmos.color = new Color( 0.2f, 0.2f, 0.2f, 0.5f );
			Gizmos.DrawCube( Vector3.zero, Vector3.one / 3 );

			Gizmos.matrix = Matrix4x4.zero;
			Gizmos.color = Color.white;
		}

		// Fields

		[SerializeField]
		internal Sheet[] properties;

		[SerializeField]
		internal bool disabled;

		[SerializeField]
		internal new string name;

		[SerializeField]
		internal string className;
	}
}
