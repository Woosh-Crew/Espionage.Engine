using System;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public interface IMapProvider
	{
		// Id
		string Identifier { get; }

		// Outcome
		Scene? Scene { get; }

		// Loading Meta
		float Progress { get; }
		bool IsLoading { get; }

		// Resource
		void Load( Action finished );
		void Unload( Action finished );
	}
}
