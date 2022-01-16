using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	[Title( "Blueprint" ), Spawnable( true ), Library.Constructor( nameof( Constructor ) )]
	[CreateAssetMenu( menuName = "Espionage.Engine/Blueprint", fileName = "Blueprint" )]
	public sealed class Blueprint : ScriptableObject, ILibrary
	{
		[field: SerializeField]
		public Library ClassInfo { get; set; }
		private bool original = true;

		public void Cache()
		{
			try
			{
				if ( !string.IsNullOrEmpty( ClassInfo.name ) )
				{
					ClassInfo.Class = typeof( Blueprint );
					Library.Database.Add( ClassInfo );
				}
			}
			catch ( Exception e )
			{
				Debugging.Log.Exception( e );
			}
		}

		public Blueprint Spawn()
		{
			if ( !original )
			{
				Debugging.Log.Warning( $"Cannot Spawn {this}, because its not an original" );
				return null;
			}

			var newBp = ScriptableObject.Instantiate( this );
			newBp.original = false;

			return newBp;
		}

		private static ILibrary Constructor( Library library )
		{
			// Pull name from database, and return an instance of it.
			return null;
		}
	}
}
