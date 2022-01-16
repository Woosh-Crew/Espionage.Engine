using System;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	[Title( "Blueprint" ), Spawnable( true ), Library.Constructor( nameof(Constructor) )]
	public sealed class Blueprint : ScriptableObject, ILibrary
	{
		public Library ClassInfo { get; set; }

		public void Cache()
		{
			try
			{
				if ( string.IsNullOrEmpty( ClassInfo.Name ) )
				{
					return;
				}

				ClassInfo.Class = typeof(Blueprint);
				Library.Database.Add( ClassInfo );
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}

		private bool _original = true;

		public Blueprint Spawn()
		{
			if ( !_original )
			{
				Debugging.Log.Warning( $"Cannot Spawn {this}, because its not an original" );
				return null;
			}

			var newBp = Instantiate( this );
			newBp._original = false;

			return newBp;
		}

		private static ILibrary Constructor( Library library )
		{
			// Pull name from database, and return an instance of it.
			return null;
		}
	}
}
