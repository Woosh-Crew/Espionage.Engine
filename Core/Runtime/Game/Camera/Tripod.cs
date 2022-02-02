using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Tripod is a camera controller. It controls the Main Camera in the game.
	/// </summary>
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
			get => _setup.Position;
			set => _setup.Position = value;
		}

		protected Quaternion Rotation
		{
			get => _setup.Rotation;
			set => _setup.Rotation = value;
		}

		protected float FieldOfView
		{
			get => _setup.FieldOfView;
			set => _setup.FieldOfView = value;
		}

		protected abstract void Update();

		public void Build( ref Setup camSetup )
		{
			_setup = camSetup;

			Update();

			if ( camSetup.FieldOfView == 0 )
			{
				camSetup.FieldOfView = 90;
			}
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
