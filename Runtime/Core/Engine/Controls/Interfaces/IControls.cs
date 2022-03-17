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
		void Build( Controls.Setup setup );
	}
}
