using Espionage.Engine;
using Mirror;
using UnityEngine;

namespace Espionage.Engine
{
	public sealed class Client : Entity
	{
		public string Name { get; }
		public long Id { get; }

		public override string ToString() => $"{Name} - {Id}\nPawn: {Pawn}, Camera: {Camera}";

		public override void OnStartAuthority()
		{
			Local.Client = this;
		}

		[SyncVar]
		public int SyncTest;

		public override void Spawn()
		{
			SyncTest = 10;
		}

		//
		// Pawn
		//

		public Pawn Pawn { get; private set; }

		public void AssignPawn( Pawn newPawn, bool destoryOriginal = true, bool respawn = true )
		{
			if ( destoryOriginal && Pawn is not null )
				GameObject.Destroy( Pawn );

			Pawn?.UnPossess();
			Pawn = newPawn;
			Pawn?.Possess();

			if ( respawn )
				Pawn.Respawn();
		}

		//
		// Camera
		//

		public MonoBehaviour Camera { get; private set; }
		public MonoBehaviour DeveloperCamera { get; private set; }
	}
}


public static class ClientExtensions
{
	public static T AssignPawn<T>( this Client cl, bool destoryOriginal = true, bool respawn = true ) where T : Pawn, new()
	{
		var pawn = Library.Creator.Create<T>();
		cl.AssignPawn( pawn, destoryOriginal, respawn );
		return pawn;
	}
}
