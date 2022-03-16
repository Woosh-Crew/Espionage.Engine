using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Tripods;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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

			// Explosions, Landing, etc
			Effect.Apply( ref setup );
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

			foreach ( var effect in Components.GetAll<Modifier>() )
			{
				effect.PostCameraSetup( ref setup );
			}
		}

		[Group( "Viewmodels" )]
		public abstract class Modifier : Component<Viewmodel>
		{
			public abstract void PostCameraSetup( ref Tripod.Setup setup );
		}

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
		private bool castShadows = false;

		[SerializeField]
		private bool receiveShadows = false;
	}
}
