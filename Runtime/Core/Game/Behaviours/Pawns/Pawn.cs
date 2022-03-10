using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Pawns can be possessed by clients, and allows the game to flow. By
	/// default Pawns have a Controller and Tripod.
	/// </summary>
	[Group( "Pawns" )]
	public class Pawn : Entity, ISimulated, IControls
	{
		protected override void OnAwake()
		{
			Tripod = GetComponent<ITripod>();
		}

		public Vector3 Velocity { get; set; }

		public void Simulate( Client client )
		{
			// EyePos & EyeRot

			var input = client.Input;
			EyeRot = Quaternion.Euler( input.ViewAngles );
			EyePos = transform.position + Vector3.Scale( Vector3.up, transform.lossyScale ) * eyeHeight;

			transform.localRotation = Quaternion.AngleAxis( EyeRot.eulerAngles.y, Vector3.up );

			// Controller

			GetActiveController()?.Simulate( client, this );

			// Components

			foreach ( var item in Components.GetAll<ISimulated>() )
			{
				// Don't simulate Pawn Controllers, we do that above.
				if ( item is IController )
				{
					continue;
				}

				item.Simulate( client );
			}
		}

		//
		// Pawn
		//

		public Client Client { get; set; }

		public Vector3 EyePos { get; set; }
		public Quaternion EyeRot { get; set; }

		public virtual void Posses( Client client )
		{
			Controller ??= GetComponent<IController>();

			foreach ( var item in Components.All.OfType<ICallbacks>() )
			{
				item.Possess( client );
			}
		}

		public virtual void UnPosses()
		{
			Controller = null;

			foreach ( var item in Components.All.OfType<ICallbacks>() )
			{
				item.UnPossess();
			}
		}

		//
		// Controller
		//

		private IController GetActiveController()
		{
			return DevController ?? Controller;
		}

		/// <summary>
		/// The controller that is used
		/// for controlling this pawn.
		/// </summary>
		public IController Controller { get; set; }

		/// <summary>
		/// This controller will override the normal controller.
		/// Is used for dev shit like no clip.
		/// </summary>
		public IController DevController { get; set; }

		//
		// Camera
		//

		public ITripod Tripod { get; set; }

		public void PostCameraSetup( ref ITripod.Setup setup ) { }

		//
		// Callbacks
		//

		public interface IController
		{
			void Simulate( Client client, Pawn pawn );
		}

		public interface ICallbacks
		{
			void Possess( Client client );
			void UnPossess();
		}

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

		/// <summary> The Position of this Entity. </summary>
		public Vector3 Position { get => transform.position; set => transform.position = value; }

		/// <summary> The Rotation of this Entity. </summary>
		public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

		/// <summary> The Local Scale of this Entity. </summary>
		public Vector3 Scale { get => transform.localScale; set => transform.localScale = value; }

		/// <summary> Is this currently being possessed? </summary>
		public bool IsClient => Client != null;

		// Fields

		[SerializeField]
		private float eyeHeight = 1.65f;
	}
}
