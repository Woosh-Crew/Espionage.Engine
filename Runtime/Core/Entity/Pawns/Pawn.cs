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

		protected override void OnAwake()
		{
			gameObject.layer = LayerMask.NameToLayer( "Pawn" );
		}

		public void Simulate( Client client )
		{
			GetActiveController()?.Simulate( client );

			foreach ( var item in Components.GetAll<ISimulated>() )
			{
				item.Simulate( client );
			}
		}

		//
		// Pawn
		//

		[Property] public Vector3 EyePos { get; internal set; }
		[Property] public Quaternion EyeRot { get; internal set; }

		public virtual void Posses( Client client )
		{
			PawnController ??= Components.Get<Controller>();
			Tripod ??= Components.Get<Tripod>();

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Possess( client );
			}
		}

		public virtual void UnPossess()
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
			return DevController ?? PawnController;
		}

		/// <summary>
		/// The controller that is currently being used
		/// for controlling this pawn.
		/// </summary>
		[Property] public Controller PawnController { get; set; }

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev shit like no-clip.
		/// </summary>
		[Property] public Controller DevController { get; set; }

		//
		// Camera
		//

		[Property] public Tripod Tripod { get; set; }

		public virtual void PostCameraSetup( ref Tripod.Setup setup )
		{
			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.PostCameraSetup( ref setup );
			}
		}

		//
		// Controls
		//

		void IControls.Build( Controls.Setup setup )
		{
			OnBuildControls( setup );

			foreach ( var item in Components.GetAll<IControls>() )
			{
				item.Build( setup );
			}
		}

		protected virtual void OnBuildControls( Controls.Setup setup ) { }

		//
		// Helpers
		//

		/// <summary>
		/// The Visuals is what gets assigned to on the Viewer
		/// on a tripod, when updating the camera. This will just
		/// disable all Renderers in its children tree.
		/// </summary>
		public Transform Visuals => visuals;

		/// <summary>
		/// Is this pawn currently being possessed
		/// by a client?
		/// </summary>
		public bool IsClient => Client != null;

		/// <summary>
		/// Component Callbacks Specific for this Entity.
		/// Use this interface on a Pawn component if you
		/// wanna have pawn specific callbacks.
		/// </summary>
		public interface ICallbacks
		{
			/// <inheritdoc cref="Pawn.Posses"/>
			void Possess( Client client ) { }

			/// <inheritdoc cref="Pawn.UnPossess"/>
			void UnPossess() { }

			/// <inheritdoc cref="Pawn.PostCameraSetup"/>
			void PostCameraSetup( ref Tripod.Setup setup ) { }
		}

		// Fields

		[SerializeField]
		private Transform visuals;
	}
}
