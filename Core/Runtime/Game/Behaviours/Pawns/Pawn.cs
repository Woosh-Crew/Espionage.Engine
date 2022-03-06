using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Pawns can be possessed by clients,
	/// and allows the game to flow. By
	/// default Pawns have a Controller and
	/// Tripod.
	/// </summary>
	[Group( "Pawns" )]
	public class Pawn : Entity, ISimulated
	{
		protected override void OnAwake()
		{
			base.OnAwake();

			// Lets find a tripod on the object
			Tripod = GetComponent<ITripod>();
		}

		public void Simulate( Client client )
		{
			GetActiveController()?.Simulate( client );
		}

		//
		// Pawn
		//

		public Vector3 EyePos { get; set; }
		public Quaternion EyeRot { get; set; }

		public virtual void Posses( Client client ) { }
		public virtual void UnPosses() { }

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
				((IComponent<Pawn>)comp)?.OnAttached( this );
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

		//
		// Camera
		//

		public ITripod Tripod { get; set; }

		public void PostCameraSetup( ref ITripod.Setup setup ) { }
	}
}
