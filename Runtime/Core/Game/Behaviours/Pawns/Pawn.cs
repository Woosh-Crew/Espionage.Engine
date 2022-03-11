using System.Linq;
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
		protected override void OnAwake()
		{
			Tripod = GetComponent<ITripod>();
		}

		public Vector3 Velocity { get; set; }

		public void Simulate( Client client )
		{
			// EyePos & EyeRot

			EyeRot = Quaternion.Euler( client.Input.ViewAngles );
			EyePos = Position + Vector3.Scale( Vector3.up, Scale ) * eyeHeight;

			Rotation = Quaternion.AngleAxis( EyeRot.eulerAngles.y, Vector3.up );

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

		public Vector3 EyePos { get; set; }
		public Quaternion EyeRot { get; set; }

		public virtual void Posses( Client client )
		{
			CurrentController ??= Components.Get<Controller>();

			foreach ( var item in Components.All.OfType<ICallbacks>() )
			{
				item.Possess( client );
			}
		}

		public virtual void UnPosses()
		{
			CurrentController = null;

			foreach ( var item in Components.All.OfType<ICallbacks>() )
			{
				item.UnPossess();
			}
		}

		//
		// Controller
		//

		private Controller GetActiveController()
		{
			return DevController ? DevController : CurrentController;
		}

		/// <summary>
		/// The controller that is used
		/// for controlling this pawn.
		/// </summary>
		public Controller CurrentController { get; set; }

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev shit like no clip.
		/// </summary>
		public Controller DevController { get; set; }

		//
		// Camera
		//

		public ITripod Tripod { get; set; }

		public void PostCameraSetup( ref ITripod.Setup setup ) { }

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
		private float eyeHeight = 1.65f;
	}
}
