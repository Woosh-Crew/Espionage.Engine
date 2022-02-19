﻿using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class PawnController : Behaviour, IComponent<Pawn>
	{
		void IComponent<Pawn>.OnAttached( Pawn item )
		{
			Pawn = item;
		}

		public Pawn Pawn { get; private set; }

		public override void Simulate( Client client )
		{
			var input = client.Input;

			Pawn.EyeRot = Quaternion.Euler( input.ViewAngles );
			Pawn.EyePos = transform.position + Vector3.up * eyeHeight;

			transform.localRotation = Quaternion.AngleAxis( Pawn.EyeRot.eulerAngles.y, Vector3.up );
		}

		// Fields

		[SerializeField]
		private float eyeHeight = 1.65f;
	}
}
