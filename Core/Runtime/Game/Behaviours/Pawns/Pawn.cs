using System.Linq;
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
			// Lets find a tripod on the object
			Tripod = GetComponent<ITripod>();
			Controller = GetComponent<PawnController>();
		}

		public void Simulate( Client client )
		{
			GetActiveController()?.Simulate( client );

			foreach ( var item in Components.GetAll<ISimulated>() )
			{
				// Don't simulate Pawn Controllers, we do that above.
				if ( item is PawnController )
				{
					continue;
				}

				item.Simulate( client );
			}
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

		/// <summary>
		/// The controller that is used
		/// for controlling this pawn.
		/// </summary>
		public PawnController Controller { get; set; }

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
