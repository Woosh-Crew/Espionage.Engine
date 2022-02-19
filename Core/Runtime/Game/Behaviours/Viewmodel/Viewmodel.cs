using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	public sealed class Viewmodel : Behaviour
	{
		public static List<Viewmodel> All { get; } = new();

		protected override void OnAwake()
		{
			All.Add( this );
			Effects = GetComponents<IEffect>().ToList();

			foreach ( var render in GetComponentsInChildren<Renderer>() )
			{
				render.shadowCastingMode = ShadowCastingMode.Off;
				render.gameObject.layer = LayerMask.NameToLayer( "Viewmodel" );
			}
		}

		protected override void OnDelete()
		{
			All.Remove( this );
		}

		public List<IEffect> Effects { get; private set; }

		public void PostCameraSetup( ref ITripod.Setup setup )
		{
			if ( Local.Client.Tripod != null )
			{
				return;
			}

			var trans = transform;
			trans.localPosition = setup.Position;
			trans.localRotation = setup.Rotation;

			foreach ( var effect in Effects )
			{
				effect.PostCameraSetup( ref setup );
			}
		}

		public interface IEffect
		{
			void PostCameraSetup( ref ITripod.Setup setup );
		}
	}
}
