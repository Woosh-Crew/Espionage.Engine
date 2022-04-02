using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Used by the pawn to represent where its
	/// eyes are. AI and Tripods use this for
	/// vision placement (Camera /  Sense)
	/// </summary>
	public struct Eyes
	{
		internal Eyes( Vector3 pos, Quaternion rot )
		{
			Position = pos;
			Rotation = rot;
		}

		public Vector3 Position { get; internal set; }
		public Quaternion Rotation { get; internal set; }

		public Trace.Builder Ray( float distance = 0.9f )
		{
			return Trace.Ray( Position, Rotation.Forward(), distance ).Ignore( "Pawn" );
		}
	}
}
