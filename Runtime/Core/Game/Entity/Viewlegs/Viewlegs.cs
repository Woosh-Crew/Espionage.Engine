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
				if ( viewmodel.GameObject.activeInHierarchy )
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
				viewmodel.GameObject.SetActive( value );
			}
		}

		// Instance

		protected override void OnSpawn()
		{
			Visuals.Changed += OnModelChanged;
			Enabled = Showing;	
		}

		private void OnModelChanged()
		{
			foreach ( var render in Visuals.Renderers )
			{
				render.shadowCastingMode = ShadowCastingMode.Off;
				render.receiveShadows = false;
				render.gameObject.layer = LayerMask.NameToLayer( "Viewmodel" );

				// Assign Correct Viewmodel Shader
				foreach ( var mat in render.materials )
				{
					mat.shader = Shader.Find( "Viewmodel Standard" );
				}
			}
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

			var trans = Transform;
			trans.localPosition = pawn.Position;
			trans.localRotation = pawn.Rotation;
			trans.localScale = pawn.Scale;
		}
	}
}
