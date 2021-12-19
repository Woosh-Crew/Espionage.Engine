// We will use this class for base Entity stuff..
// This includes pawns, the event system, etc

using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class Entity : NetworkBehaviour, ILibrary
	{
		//
		// Network Registering
		//

		internal static void Register( Library library )
		{
			NetworkClient.RegisterSpawnHandler( library.Id, SpawnEntity, UnspawnEntity );
		}

		private static GameObject SpawnEntity( SpawnMessage msg )
		{
			var spawned = Library.Creator.Create( msg.assetId );
			spawned.transform.position = msg.position;
			spawned.transform.rotation = msg.rotation;
			spawned.transform.localScale = msg.scale;

			Debug.Log( $"Spawning Entity {spawned.ClassInfo.Name}" );
			spawned.ClientSpawn();
			return spawned.gameObject;
		}

		private static void UnspawnEntity( GameObject spawned )
		{
			spawned.GetComponent<Entity>().ClientDestory();
			GameObject.Destroy( spawned );
		}

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
		public virtual void Simulate( Client cl ) { }

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

		//
		// Helpers
		//
		public Library ClassInfo => Library.Accessor.Get( GetType() );

		public Pawn Owner { get; set; }
		public Vector3 Position => transform.position;
		public Quaternion Rotation => transform.rotation;
		public Vector3 Scale => transform.localScale;

		//
		// Logic
		//

		private void Update()
		{
			if ( netIdentity is not null && IsServer )
				ServerUpdate();
		}

		private void FixedUpdate()
		{
		}

		private void ServerUpdate()
		{
			// Think
			if ( ThinkDelay > 0 && (NetworkTime.time - lastThinkDelay) >= ThinkDelay )
				Think( (float)(NetworkTime.time - lastThinkDelay) );
		}
	}
}
