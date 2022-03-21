using System.Collections.Generic;
using Espionage.Engine.Tripods;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// A Tripod is Espionage.Engines camera system.
	/// It acts kinda like a wrapper for a singleton camera
	/// pattern, Which allows us to be super performant.
	/// </para>
	/// <para>
	/// You use the Tripod for manipulating the Main Camera
	/// every frame (Called in the Late Update Loop). You can easily
	/// switch out tripods by replacing the <see cref="Pawn"/>'s
	/// tripod, or overriding the <see cref="Client"/>'s tripod entirely.
	/// </para>
	/// </summary>
	public interface ITripod : ILibrary
	{
		/// <summary>
		/// Called every Tripod update. Used
		/// to control the main camera, without
		/// any overhead of complications.
		/// </summary>
		void Build( ref Tripod.Setup camSetup );

		/// <summary>
		/// Called when the Tripod is now the active one, use this
		/// for snapping your tripod to its initial position and rotation.
		/// </summary>
		void Activated( ref Tripod.Setup camSetup );

		/// <summary>
		/// Called when the tripod goes out of use. Use this for cleaning
		/// up resources if need be.
		/// </summary>
		void Deactivated();

	}
}
