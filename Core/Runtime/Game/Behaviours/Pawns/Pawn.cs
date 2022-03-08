using System.Linq;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Pawns can be possessed by clients, and allows the game to flow. By
	/// default Pawns have a Controller and Tripod.
	/// </summary>
	[Group( "Pawns" )]
	public class Pawn : Entity, ISimulated
	{
		protected override void OnAwake()
		{
			Tripod = GetComponent<ITripod>();
			Controller = GetComponent<IController>();
		}

		public void Simulate( Client client )
		{
			// EyePos & EyeRot

			var input = client.Input;

			EyeRot = Quaternion.Euler( input.ViewAngles );
			EyePos = transform.position + Vector3.Scale( Vector3.up, transform.lossyScale ) * eyeHeight;

			transform.localRotation = Quaternion.AngleAxis( EyeRot.eulerAngles.y, Vector3.up );

			// Controller

			GetActiveController()?.Simulate( client );

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

		public Vector3 EyePos { get; set; }
		public Quaternion EyeRot { get; set; }

		public virtual void Posses( Client client ) { }
		public virtual void UnPosses() { }

		//
		// Controller
		//

		public interface IController : ISimulated { }

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


		// Fields

		[SerializeField]
		private float eyeHeight = 1.65f;
	}
}
