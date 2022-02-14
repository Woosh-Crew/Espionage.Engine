using System;

namespace Espionage.Engine.Resources
{
	public interface IModelProvider : IResource
	{
		// Loading Meta
		float Progress { get; }
	}
}
