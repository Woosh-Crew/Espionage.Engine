using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// A Client is a person that is currently playing the game.
	/// Clients controls Input and their current possessed Pawn.
	/// </summary>
	[Group( "Networking" ), Spawnable( false )]
	public class Client : Entity
	{
		public new static IEnumerable<Client> All => Entity.All.OfType<Client>();

		internal static Client Create( string name )
		{
			var obj = new GameObject( $"[id=0]{name}" ).AddComponent<Client>();
			Engine.Scene.Move( obj.gameObject );
			return obj;
		}

		// Instance

		/// <summary> A Nice name for the Client. </summary>
		public string Name { get; set; }

		/// <summary> Is this client ready to enter the game world? </summary>
		public bool IsReady { get; internal set; }

		internal virtual void Simulate()
		{
			Engine.Game.Simulate( this );
		}

		// Camera

		/// <summary>
		/// The clients active tripod.
		/// This overrides the current pawns tripod
		/// </summary>
		public ITripod Tripod { get; set; }

		// Input

		/// <summary>
		/// The clients current input buffer.
		/// <remarks>
		/// you should be using this instead of Unity's default
		/// input system.
		/// </remarks>
		/// </summary>
		public IControls.Setup Input { get; internal set; }

		// Pawn

		private Pawn _pawn;

		/// <summary> The pawn this client is currently possessing. </summary>
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
