using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// IControls, controls the flow of the input System.
	/// Add this to your class and call it in the Game's
	/// on build input. 
	/// </para>
	/// <para>
	/// The Flow goes like this. Active Tripod >
	/// Active Pawn. In the future we will add more
	/// to this flow.
	/// </para>
	/// </summary>
	public interface IControls
	{
		/// <summary>
		/// Gets called every frame before anything happens.
		/// This allows us to control the input flow per object,
		/// allows us to do cool shit like disabling all input
		/// while using a camera ( We use this on <see cref="Tripods.DevTripod"/>
		/// </summary>
		void Build( Setup setup );

		/// <summary> Controls the raw values of Input. </summary>
		public class Setup
		{
			/// <summary> The Current Mouse Delta </summary>
			public Vector2 MouseDelta { get; internal set; }

			/// <summary> The Current Mouse Delta </summary>
			public float MouseWheel { get; internal set; }

			/// <summary> Forward Direction </summary>
			public float Forward { get; internal set; }

			/// <summary> Horizontal Direction </summary>
			public float Horizontal { get; internal set; }

			/// <summary> Where a pawns Eyes should be facing (Angles) </summary>
			public Vector3 ViewAngles { get; set; }

			/// <summary> Clears the Input Setup </summary>
			public void Clear()
			{
				MouseDelta = Vector2.zero;
				MouseWheel = 0;

				Forward = 0;
				Horizontal = 0;
			}
		}
	}
}
