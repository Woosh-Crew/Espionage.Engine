using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	public sealed class Viewmodel : Entity
	{
		public static void Apply( ref ITripod.Setup setup )
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
			foreach ( var viewmodel in All )
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
				render.gameObject.layer = LayerMask.NameToLayer( "Viewmodel" );
			}
		}

		protected override void OnDelete()
		{
			All.Remove( this );
		}

		public void PostCameraSetup( ref ITripod.Setup setup )
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

		public abstract class Effect : Component<Viewmodel>
		{
			public abstract void PostCameraSetup( ref ITripod.Setup setup );
		}

		// Fields

		[SerializeField]
		private bool castShadows = false;

		[SerializeField]
		private bool receiveShadows = false;
	}
}
