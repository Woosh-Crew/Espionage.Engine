using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Espionage.Engine
{
	/// <summary>
	/// Viewmodels are what show up in First Person, when the
	/// current pawn is carrying something.
	/// </summary>
	[Group( "Viewmodels" )]
	public sealed class Viewmodel : Entity
	{
		public static void Apply( ref Tripod.Setup setup )
		{
			SetMatrix( setup );

			// Build Viewmodels...
			foreach ( var viewmodel in All.OfType<Viewmodel>() )
			{
				if ( viewmodel.gameObject.activeInHierarchy )
				{
					viewmodel.PostCameraSetup( ref setup );
				}
			}

			// Explosions, Landing, etc
			Viewlegs.Apply( ref setup );
			Effect.Apply( ref setup );
		}

		private static bool Showing { get; set; }

		public static void Show( bool value )
		{
			Viewlegs.Show( value );

			Showing = value;

			foreach ( var viewmodel in All.OfType<Viewmodel>() )
			{
				viewmodel.gameObject.SetActive( value );
			}
		}

		// Projection Matrix

		private static void SetMatrix( Tripod.Setup setup )
		{
			var viewmodel = setup.Viewmodel;

			var matrix = Matrix4x4.Perspective( viewmodel.FieldOfView, setup.Camera.aspect, viewmodel.Clipping.x, viewmodel.Clipping.y );
			var view = setup.Camera.transform.worldToLocalMatrix;

			Shader.SetGlobalMatrix( "_PrevCustomVP", matrix * view );
			Shader.SetGlobalMatrix( "_CustomProjMatrix", GL.GetGPUProjectionMatrix( matrix, false ) );
		}

		// Instance

		protected override void OnAwake()
		{
			foreach ( var render in GetComponentsInChildren<Renderer>() )
			{
				render.shadowCastingMode = castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
				render.receiveShadows = receiveShadows;
				render.gameObject.layer = LayerMask.NameToLayer( "Viewmodel" );

				// Assign Correct Viewmodel Shader
				foreach ( var mat in render.materials )
				{
					mat.shader = Shader.Find( "Viewmodel Standard" );
				}
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

			foreach ( var effect in Components.GetAll<Modifier>() )
			{
				effect.PostCameraSetup( ref setup );
			}
		}

		//
		// Classes
		// 

		/// <summary>
		/// Viewmodel Modifiers are components that get attached to
		/// the viewmodel entity, allowing it to be changed. You
		/// would use this for sway, bob, breathe, etc.
		/// </summary>
		[Group( "Viewmodels" )]
		public abstract class Modifier : Component<Viewmodel>
		{
			public Vector3 Position { get => Entity.Position; set => Entity.Position = value; }
			public Quaternion Rotation { get => Entity.Rotation; set => Entity.Rotation = value; }

			public abstract void PostCameraSetup( ref Tripod.Setup setup );
		}

		/// <summary>
		/// Viewmodel.Effect is a temporary viewmodel modifier. We
		/// can use this for Landing Effects, Explosions, Animations,
		/// etc. Basically allows you to do cool shit
		/// </summary>
		public abstract class Effect
		{
			private static readonly List<Effect> All = new();

			public static void Apply( ref Tripod.Setup setup )
			{
				for ( var i = All.Count; i > 0; i-- )
				{
					var remove = false;

					foreach ( var viewmodel in All.OfType<Viewmodel>() )
					{
						if ( All[i - 1].Update( ref setup, viewmodel ) )
						{
							remove = true;
						}
					}

					if ( remove )
					{
						All.RemoveAt( i - 1 );
					}
				}
			}

			public static void Clear()
			{
				All.Clear();
			}

			public Effect()
			{
				All.Add( this );
			}

			/// <returns> True if were done with this Modifier </returns>
			protected abstract bool Update( ref Tripod.Setup setup, Viewmodel viewmodel );
		}

		// Fields

		[SerializeField]
		private bool castShadows;

		[SerializeField]
		private bool receiveShadows;
	}
}
