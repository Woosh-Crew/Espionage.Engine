using UnityEngine;

namespace Espionage.Engine
{
	public interface ICamera
	{
		void Build( ref Setup camSetup );

		void Activated( Setup camSetup );
		void Deactivated();

		public struct Setup
		{
			// Camera
			public float FieldOfView;
			public GameObject Viewer;

			// Transform
			public Vector3 Position;
			public Quaternion Rotation;
		}
	}
}
