using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Tripod is a camera controller behaviour. It controls the Main Camera in the game.
	/// </summary>
	public abstract class Tripod : Behaviour, ICamera
	{
		public virtual void Activated( ICamera.Setup camSetup ) { }
		public virtual void Deactivated() { }

		//
		// Camera Building
		//

		private ICamera.Setup _setup;

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

		protected abstract void Frame();

		public void Build( ref ICamera.Setup camSetup )
		{
			_setup = camSetup;

			Frame();

			if ( camSetup.FieldOfView == 0 )
			{
				camSetup.FieldOfView = 90;
			}
		}
	}
}
