using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public class Pawn : Behaviour
	{
		public Tripod Tripod { get; protected set; }

		private void Update()
		{
			GetActiveController()?.Simulate();
		}

		//
		// Controller
		//

		protected virtual PawnController GetActiveController()
		{
			return DevController ? DevController : Controller;
		}

		private PawnController _controller;

		public PawnController Controller
		{
			get
			{
				if ( _controller != null )
				{
					return _controller;
				}

				var comp = GetComponent<PawnController>();
				if ( comp == null )
				{
					return null;
				}

				_controller = comp;
				return _controller;
			}
			set
			{
				if ( _controller != null )
				{
					Destroy( _controller );
				}

				if ( value.gameObject != gameObject )
				{
					Debugging.Log.Error( "New Controller GameObject isn't on Pawn GameObject" );
					return;
				}

				((IComponent<Pawn>)value)?.OnAttached( this );
				_controller = value;
			}
		}

		public PawnController DevController { get; set; }
	}
}
