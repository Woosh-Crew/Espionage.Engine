using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public class Client : IDisposable
	{
		public static List<Client> All { get; } = new ();

		public Client()
		{
			All.Add( this );
		}
		
		public void Dispose()
		{
			All.Remove( this );
		}
		
		// Meta
		
		public string Name { get; set; }
		public ulong Id { get; set; }
		
		// Pawn
		
		private Pawn _pawn;

		public Pawn Pawn
		{
			get
			{
				return _pawn;
			}
			set
			{
				if ( _pawn != null )
				{
					_pawn.UnPosses();
				}

				_pawn = value;
				_pawn.Posses();
			}
		}
	}
}
