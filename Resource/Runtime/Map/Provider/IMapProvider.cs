using System;
using UnityEngine.SceneManagement;

namespace Espionage.Engine.Resources
{
	public interface IMapProvider : IResource
	{
		Scene? Scene { get; }
		float Progress { get; }
	}
}
