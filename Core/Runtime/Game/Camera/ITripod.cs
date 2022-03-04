using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public interface ITripod
	{
		void Build( ref Setup camSetup );

		void Activated( ref Setup camSetup );
		void Deactivated();

		public struct Setup
		{
			// Camera

			///< summary> Camera's FOV </summary>
			public float FieldOfView;

			///< summary> FieldOfView Damping </summary>
			public float Damping;

			/// <summary> Will hide nested Renderers if set </summary>
			public Transform Viewer;

			// Transform
			public Vector3 Position;
			public Quaternion Rotation;

			/// <summary> Clipping Planes, X = Near, Y = Far </summary>
			public Vector2 Clipping;
		}

		public abstract class Modifier
		{
			private static List<Modifier> All = new();

			public static void Apply( ref Setup setup )
			{
				for ( var i = All.Count; i > 0; i-- )
				{
					var keep = All[i - 1].Update( ref setup );

					if ( !keep )
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
