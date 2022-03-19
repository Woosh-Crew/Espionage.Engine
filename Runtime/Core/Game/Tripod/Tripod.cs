using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// <inheritdoc cref="ITripod"/>
	/// <para>
	/// This class is just a component inherited from <see cref="Component{Pawn}"/>  with the ITripod. This allows you
	/// to control parameters in editor, and use Unity's components workflow. The <see cref="Pawn"/> will
	/// ask for a Tripod Component too if none has been previously declared on it.
	/// </para>
	/// </summary>
	[Group( "Tripods" )]
	public abstract class Tripod : Component<Pawn>, ITripod, IControls
	{
		// Tripod

		/// <summary> <inheritdoc cref="ITripod.Activated"/> </summary>
		public virtual void Activated( ref Setup camSetup ) { }

		/// <summary> <inheritdoc cref="ITripod.Deactivated"/> </summary>
		public virtual void Deactivated() { }

		void ITripod.Build( ref Setup camSetup )
		{
			OnBuildTripod( ref camSetup );
		}

		/// <summary> <inheritdoc cref="ITripod.Build"/> </summary>
		protected virtual void OnBuildTripod( ref Setup setup ) { }

		// Controls

		void IControls.Build( Controls.Setup setup )
		{
			OnBuildControls( setup );
		}

		/// <summary> <inheritdoc cref="IControls.Build"/> </summary>
		protected virtual void OnBuildControls( Controls.Setup setup )
		{
			setup.ViewAngles += new Vector3( -setup.Mouse.Delta.y, setup.Mouse.Delta.x, 0 );
			setup.ViewAngles = setup.ViewAngles.WithX( Mathf.Clamp( setup.ViewAngles.x, -88, 88 ) );
		}

		/// <summary>
		/// A Tripod.Setup is responsible for controlling how the
		/// main camera gets manipulated in the world. Tripod setups
		/// are built every frame (In the Late Update Loop).
		/// </summary>
		public struct Setup
		{
			/// <summary> Camera's FOV </summary>
			public float FieldOfView;

			/// <summary> FieldOfView Damping </summary>
			public float Damping;

			/// <summary> Will hide nested Renderers if set </summary>
			public Transform Viewer;

			/// <summary> The position of the camera </summary>
			public Vector3 Position;

			/// <summary> The rotation of the camera </summary>
			public Quaternion Rotation;

			/// <summary> Clipping Planes, X = Near, Y = Far </summary>
			public Vector2 Clipping;

			/// <summary> The camera rendering this tripod </summary>
			public Camera Camera { get; internal set; }
		}

		/// <summary>
		/// Tripod modifiers are temporary modifiers that change
		/// the Tripod setup after all the tripods and viewmodels have
		/// been built. This allows you to do cool stuff like
		/// screen shake, or on land effects.
		/// </summary>
		public abstract class Effect
		{
			private static readonly List<Effect> All = new();

			public static void Apply( ref Setup setup )
			{
				for ( var i = All.Count; i > 0; i-- )
				{
					var remove = All[i - 1].Update( ref setup );

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
			protected abstract bool Update( ref Setup setup );
		}
	}
}
