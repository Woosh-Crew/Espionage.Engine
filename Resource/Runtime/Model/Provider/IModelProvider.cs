using System;

namespace Espionage.Engine.Resources
{
	public interface IModelProvider : IResource, IAsset
	{
		// Loading Meta
		float Progress { get; }
	}
}
