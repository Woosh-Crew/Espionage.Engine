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
		public float Offset { get; protected set; } = 0;

		protected override void OnSpawn()
		{
			Layer = LayerMask.NameToLayer( "Pawn" );
		}

		public virtual void Simulate( Client client )
		{
			(DevController ?? PawnController)?.Simulate( client );
			Floor = Floor.Get( Position, Offset );

			foreach ( var item in Components.GetAll<ISimulated>() )
			{
				item.Simulate( client );
			}
		}

		public Floor Floor { get; private set; }
		public Eyes Eyes { get; set; }

		public virtual void Posses( Client client )
		{
			PawnController ??= Components.Get<Controller>();
			Tripod ??= Components.Get<Tripod>();

			PawnController.Enabled = true;

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.Possess( client );
			}
		}

		public virtual void UnPossess()
		{
			PawnController.Enabled = false;

			foreach ( var item in Components.GetAll<ICallbacks>() )
			{
				item.UnPossess();
			}
		}

		//
		// Controller
		//

		/// <summary>
		/// The controller that is currently being used
		/// for controlling this pawn.
		/// </summary>
		public Controller PawnController { get; set; }

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev stuff like no-clip.
		/// </summary>
		public Controller DevController { get; set; }

		//
		// Camera
		//

		public ITripod Tripod { get; set; }
		
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
		/// Is this pawn currently being possessed
		/// by a client?
		/// </summary>
		public bool IsClient => Client != null;

		public bool IsLocalPawn => Local.Client.Pawn == this;

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
	}
}
