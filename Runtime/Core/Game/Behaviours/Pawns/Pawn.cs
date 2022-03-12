using System.Linq;
using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Pawns can be possessed by clients, and allows the game to flow. By
	/// default Pawns have a Controller and Tripod.
	/// </summary>
	[Group( "Pawns" )]
	public partial class Pawn : Entity, ISimulated, IControls
	{
		public Vector3 Velocity { get; set; }

		public void Simulate( Client client )
		{
			// Controller

			GetActiveController()?.Simulate( client );

			// Components

			foreach ( var item in Components.GetAll<ISimulated>() )
			{
				// Don't simulate Pawn Controllers, we do that above.
				if ( item is Controller )
				{
					continue;
				}

				item.Simulate( client );
			}
		}

		//
		// Pawn
		//

		public Vector3 EyePos { get; internal set; }
		public Quaternion EyeRot { get; internal set; }

		public virtual void Posses( Client client )
		{
			PawnController ??= Components.Get<Controller>();
			Tripod ??= Components.Get<Tripod>();

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Possess( client );
			}
		}

		public virtual void UnPosses()
		{
			PawnController = null;

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.UnPossess();
			}
		}

		//
		// Controller
		//

		private Controller GetActiveController()
		{
			return DevController ? DevController : PawnController;
		}

		/// <summary>
		/// The controller that is used
		/// for controlling this pawn.
		/// </summary>
		public Controller PawnController { get; set; }

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev shit like no clip.
		/// </summary>
		public Controller DevController { get; set; }

		//
		// Camera
		//

		public ITripod Tripod { get; set; }

		public void PostCameraSetup( ref Tripod.Setup setup ) { }

		//
		// Controls
		//

		void IControls.Build( IControls.Setup setup )
		{
			OnBuildControls( setup );
		}

		protected virtual void OnBuildControls( IControls.Setup setup ) { }

		//
		// Helpers
		//

		/// <summary>
		/// The Visuals is what gets assigned to on the Viewer
		/// on a tripod, when updating the camera. This will just
		/// disable all Renderers in its children tree.
		/// </summary>
		public Transform Visuals => visuals;

		/// <summary> Is this currently being possessed? </summary>
		public bool IsClient => Client != null;

		/// <summary>
		/// Component Callbacks Specific for this Entity.
		/// Use this interface on a Pawn component if you
		/// wanna have pawn specific callbacks.
		/// </summary>
		public interface ICallbacks
		{
			void Possess( Client client );
			void UnPossess();
		}

		// Fields

		[SerializeField]
		private Transform visuals;
	}
}
