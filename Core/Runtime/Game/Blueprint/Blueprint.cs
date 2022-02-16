using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// Blueprints are used for spawning prefabs at runtime using C#
	/// </summary>
	[Group( "Blueprints" ), Spawnable( false )]
	public abstract class Blueprint : ILibrary
	{
		public Library ClassInfo { get; private set; }

		public Blueprint()
		{
			ClassInfo = Library.Database[GetType()];

			if ( !ClassInfo.Components.TryGet<FileAttribute>( out var fileAttribute ) )
			{
				Debugging.Log.Error( $"{ClassInfo.Name} doesn't have, FileAttribute" );
				return;
			}

			Path = fileAttribute.Path;
		}

		public string Path { get; }

		public GameObject Spawn()
		{
			return null;
		}

		public void Kill() { }
	}
}
