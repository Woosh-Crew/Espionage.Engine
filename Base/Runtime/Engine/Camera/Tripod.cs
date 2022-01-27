using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class Tripod : ICamera
	{
		public virtual void Activated() { }
		public virtual void Deactivated() { }

		//
		// Camera Building
		//

		private Setup _setup;

		protected Vector3 Position
		{
			get { return _setup.Position; } 
			set { _setup.Position = value; }
		}

		protected Quaternion Rotation
		{
			get { return _setup.Rotation; } 
			set { _setup.Rotation = value; }
		}

		protected float FieldOfView
		{
			get { return _setup.FieldOfView; }
			set { _setup.FieldOfView = value; }
		}

		protected abstract void Simulate();

		public void Build( ref Setup camSetup )
		{
			Simulate();
			camSetup = _setup;

			if ( camSetup.FieldOfView == 0 )
				camSetup.FieldOfView = 90;
		}

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
