using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	[Group( "Viewmodels" )]
	public sealed class Viewlegs : Entity
	{
		public static void Apply( ref Tripod.Setup setup )
		{
			// Build Viewmodels...
			foreach ( var viewmodel in All.OfType<Viewlegs>() )
			{
				if ( viewmodel.gameObject.activeInHierarchy )
				{
					viewmodel.PostCameraSetup( ref setup );
				}
			}
		}

		private static bool Showing { get; set; }

		public static void Show( bool value )
		{
			Showing = value;

			foreach ( var viewmodel in All.OfType<Viewlegs>() )
			{
				viewmodel.gameObject.SetActive( value );
			}
		}

		// Instance

		protected override void OnAwake()
		{
			foreach ( var render in GetComponentsInChildren<Renderer>() )
			{
				render.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
				render.receiveShadows = receiveShadows;
			}

			Enabled = Showing;
		}

		private void PostCameraSetup( ref Tripod.Setup setup )
		{
			// Basically if the current tripod is not the 
			// Pawns one, don't move...
			if ( Local.Client.Tripod.Peek() != null || Local.Pawn == null )
			{
				return;
			}

			var pawn = Local.Pawn;

			var trans = transform;
			trans.localPosition = pawn.Position;
			trans.localRotation = pawn.Rotation;
			trans.localScale = pawn.Scale;
		}

		// Fields

		[SerializeField]
		private bool castShadows;

		[SerializeField]
		private bool receiveShadows;
	}
}
