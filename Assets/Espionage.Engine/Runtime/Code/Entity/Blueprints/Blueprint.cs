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

		private void Awake()
		{
			try
			{
				if ( !string.IsNullOrEmpty( ClassInfo.Name ) )
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

		private static ILibrary Constructor( Library library )
		{
			return null;
		}
	}
}
