﻿using System.Collections.Generic;
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
	public interface ITripod
	{
		/// <summary>
		/// Called every Tripod update. Used
		/// to control the main camera, without
		/// any overhead of complications.
		/// </summary>
		void Build( ref Setup camSetup );

		/// <summary>
		/// Called when the Tripod is now the active one, use this
		/// for snapping your tripod to its initial position and rotation.
		/// </summary>
		void Activated( ref Setup camSetup );

		/// <summary>
		/// Called when the tripod goes out of use. Use this for cleaning
		/// up resources if need be.
		/// </summary>
		void Deactivated();

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
		}

		/// <summary>
		/// Tripod modifiers are temporary modifiers that change
		/// the Tripod setup after all the tripods and viewmodels have
		/// been built. This allows you to do cool stuff like
		/// screen shake, or on land effects.
		/// </summary>
		public abstract class Modifier
		{
			private static readonly List<Modifier> All = new();

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

			public Modifier()
			{
				All.Add( this );
			}

			/// <returns> True if were done with this Modifier </returns>
			protected abstract bool Update( ref Setup setup );
		}
	}
}