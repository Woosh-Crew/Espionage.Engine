using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	[Group( "Networking" )]
	public class Client : Behaviour
	{
		public string Name { get; set; }

		// Constructor

		public static Client Create( string name )
		{
			var obj = new GameObject( $"[id={0}] Client :: {name}" ).AddComponent<Client>();
			Engine.AddToLayer( obj.gameObject );
			return obj;
		}

		// Input

		public IInputProcessor Input { get; internal set; }

		// Pawn

		public Pawn Pawn { get; private set; }

		public void AssignPawn( Pawn pawn )
		{
			if ( Pawn != null )
			{
				Pawn.UnPosses();
			}

			Pawn = pawn;

			if ( Pawn != null )
			{
				Pawn.Posses();
			}
		}
	}
}
