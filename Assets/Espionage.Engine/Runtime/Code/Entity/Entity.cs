// We will use this class for base Entity stuff..
// This includes pawns, the event system, etc

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Internal;

namespace Espionage.Engine
{
	[Constructor( nameof( Construct ) )]
	public abstract class Entity : MonoBehaviour, ILibrary
	{
		//
		// Spawn
		//

		/// <summary> This is called on the Server just before the Server spawns the object. 
		/// Use this to initialize values. Or run any initialization operations. </summary>
		public virtual void Spawn() { }

		/// <summary> This is called on the client when the entity spawns, 
		/// this wont be called if this entity was only created on the client.</summary>
		public virtual void ClientSpawn() { }
		internal virtual void ClientDestory() { }

		//
		// Think
		//

		private double lastThinkDelay;

		/// <summary> How long is the interval between each think tick. Setting this to zero
		/// will stop the thinking </summary>
		public virtual float ThinkDelay { get; set; }

		/// <summary> This is called on the Server every given delay. </summary>
		/// <param name="delta"> The time taken between last think </param>
		public virtual void Think( float delta ) { Debug.Log( $"Thinking with delta : {delta}" ); }

		//
		// Simulate
		//

		/// <summary> This is called both on the owning client and server every tick. </summary>
		/// <param name="cl"> The current processsing client </param>
		public virtual void Simulate() { }

		// 
		// Management
		//

		/// <summary> All the entites present in game</summary>
		public static IReadOnlyList<Entity> All => _all;
		private static List<Entity> _all = new List<Entity>();

		private void Awake()
		{
			_all.Add( this );
		}

		private void OnDestory()
		{
			_all.Remove( this );
		}

		private static Entity Construct<T>() where T : Entity, new()
		{
			Debug.Log( "Creating Entity" );
			return new GameObject( typeof( T ).FullName ).AddComponent<T>();
		}

		//
		// Helpers
		//
		public Library ClassInfo => Library.Accessor.Get( GetType() );

		public Pawn Owner { get; set; }
		public Vector3 Position => transform.position;
		public Quaternion Rotation => transform.rotation;
		public Vector3 Scale => transform.localScale;
	}
}
