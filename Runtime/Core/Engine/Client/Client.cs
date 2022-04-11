using System.Collections.Generic;
using System.Linq;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// A Client is a person that is currently playing the game.
	/// Clients controls Input and their current possessed Pawn.
	/// </summary>
	[Group( "Networking" )]
	public class Client : ILibrary
	{
		public static List<Client> All { get; } = new();

		public Library ClassInfo { get; }
		public Components<Client> Components { get; }

		internal Client( string name )
		{
			Components = new( this );

			ClassInfo = Library.Register( this );
			All.Add( this );
		}

		public void Delete()
		{
			Library.Unregister( this );
			All.Remove( this );
		}

		/// <summary> A Nice name for the Client that is used in UI. </summary>
		public string Name { get; set; }

		/// <summary> Is this client ready to enter the game world? </summary>
		public bool IsReady { get; internal set; }

		internal virtual void Simulate()
		{
			Controls.SetSetup( this );
			Engine.Game.Simulate( this );
		}

		// Camera

		/// <summary>
		/// The clients active tripod.
		/// This overrides the current pawns tripod
		/// </summary>
		public Tripod.Stack Tripod { get; } = new();

		//
		// Input
		//

		/// <summary> The clients current input buffer. </summary>
		public Controls.Setup Input { get; set; }

		//
		// Pawn
		//

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
					_pawn.UnPossess();
				}

				_pawn = value;

				if ( _pawn != null )
				{
					Input.ViewAngles = _pawn.transform.rotation.eulerAngles;

					_pawn.Client = this;
					_pawn.Posses( this );
				}
			}
		}
	}
}
