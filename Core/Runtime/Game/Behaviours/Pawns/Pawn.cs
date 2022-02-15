using System;
using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Pawns can be possessed by clients, and allows the game to flow. 
	/// </summary>
	public sealed class Pawn : Behaviour
	{
		public Tripod Tripod { get; set; }

		public override void Simulate( Client client )
		{
			GetActiveController()?.Simulate( client );
		}

		//
		// Pawn
		//

		public void Posses( Client client ) { }
		public void UnPosses() { }

		//
		// Controller
		//

		private PawnController GetActiveController()
		{
			return DevController ? DevController : Controller;
		}

		private PawnController _controller;

		/// <summary>
		/// The controller that is used for controlling this pawn.
		/// </summary>
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

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev shit like no clip.
		/// </summary>
		public PawnController DevController { get; set; }
	}
}
