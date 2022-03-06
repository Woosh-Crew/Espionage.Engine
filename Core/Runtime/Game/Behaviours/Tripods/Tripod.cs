using UnityEngine;

namespace Espionage.Engine.Tripods
{
	/// <summary>
	/// A Tripod is just an <see cref="ITripod"/>
	/// inherited from MonoBehaviour. This allows you
	/// to control parameters in editor, and use Unity's
	/// components workflow. The <see cref="Pawn"/> will
	/// ask for a Tripod Component too if none has been previously
	/// declared on it.
	/// </summary>
	[Group( "Tripods" )]
	public abstract class Tripod : Component<Pawn>, ITripod, IControls
	{
		/// <summary>
		/// The Visuals is what gets assigned to on the Viewer
		/// when updating the camera. This will just disable all
		/// Renderers in its children tree.
		/// </summary>
		protected Transform Visuals => visuals;

		// Tripod

		/// <summary> <inheritdoc cref="ITripod.Activated"/> </summary>
		public virtual void Activated( ref ITripod.Setup camSetup ) { }

		/// <summary> <inheritdoc cref="ITripod.Deactivated"/> </summary>
		public virtual void Deactivated() { }

		void ITripod.Build( ref ITripod.Setup camSetup )
		{
			OnBuildTripod( ref camSetup );
		}

		/// <summary> <inheritdoc cref="ITripod.Build"/> </summary>
		protected virtual void OnBuildTripod( ref ITripod.Setup setup ) { }

		// Controls

		void IControls.Build( ref IControls.Setup setup )
		{
			OnBuildControls( ref setup );
		}

		/// <summary> <inheritdoc cref="IControls.Build"/> </summary>
		protected virtual void OnBuildControls( ref IControls.Setup setup )
		{
			setup.ViewAngles += new Vector3( -setup.MouseDelta.y, setup.MouseDelta.x, 0 );
			setup.ViewAngles.x = Mathf.Clamp( setup.ViewAngles.x, -88, 88 );
		}

		// Fields

		[SerializeField]
		private Transform visuals;
	}
}
