using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	[Group( "Networking" )]
	public class Client : Entity
	{
		public new static IEnumerable<Client> All => Entity.All.OfType<Client>();

		// Constructor

		public static Client Create( string name )
		{
			var obj = new GameObject( $"[id=0]{name}" ).AddComponent<Client>();
			Engine.Scene.Move( obj.gameObject );
			return obj;
		}

		public string Name { get; set; }

		// Simulate

		internal virtual void Simulate()
		{
			Engine.Game.Simulate( this );
		}

		// Camera

		public ITripod Tripod { get; set; }

		// Input

		public IControls.Setup Input { get; internal set; }

		// Pawn

		private Pawn _pawn;

		public Pawn Pawn
		{
			get => _pawn;
			set
			{
				if ( _pawn != null )
				{
					_pawn.Client = null;
					_pawn.UnPosses();
				}

				_pawn = value;

				if ( _pawn != null )
				{
					_pawn.Client = this;
					_pawn.Posses( this );
				}
			}
		}
	}
}
