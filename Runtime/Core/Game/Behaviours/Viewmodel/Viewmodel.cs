using System.Linq;
using Espionage.Engine.Tripods;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	[Group( "Viewmodels" )]
	public sealed class Viewmodel : Entity
	{
		public static void Apply( ref Tripod.Setup setup )
		{
			// Build Viewmodels...
			foreach ( var viewmodel in All.OfType<Viewmodel>() )
			{
				if ( viewmodel.gameObject.activeInHierarchy )
				{
					viewmodel.PostCameraSetup( ref setup );
				}
			}
		}

		public static void Show( bool value )
		{
			Showing = value;

			foreach ( var viewmodel in All.OfType<Viewmodel>() )
			{
				viewmodel.gameObject.SetActive( value );
			}
		}

		private static bool Showing { get; set; }

		// Instance

		protected override void OnAwake()
		{
			foreach ( var render in GetComponentsInChildren<Renderer>() )
			{
				render.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
				render.receiveShadows = receiveShadows;
				render.gameObject.layer = LayerMask.NameToLayer( "Viewmodel" );
			}

			Enabled = Showing;
		}

		private void PostCameraSetup( ref Tripod.Setup setup )
		{
			// Basically if the current tripod is not the 
			// Pawns one, don't move...
			if ( Local.Client.Tripod != null )
			{
				return;
			}

			var trans = transform;
			trans.localPosition = setup.Position;
			trans.localRotation = setup.Rotation;

			foreach ( var effect in Components.GetAll<Effect>() )
			{
				effect.PostCameraSetup( ref setup );
			}
		}

		[Group( "Viewmodels" )]
		public abstract class Effect : Component<Viewmodel>
		{
			public abstract void PostCameraSetup( ref Tripod.Setup setup );
		}

		// Fields

		[SerializeField]
		private bool castShadows = false;

		[SerializeField]
		private bool receiveShadows = false;
	}
}
