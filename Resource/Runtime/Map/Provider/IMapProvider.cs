using System;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public interface IMapProvider
	{
		Scene? Scene { get; }
		bool IsLoading { get; }
		
		void Load( string path, Action finished );
		void Unload( string path, Action finished );
	}
}
