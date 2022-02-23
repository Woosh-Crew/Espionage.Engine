using System;

namespace Espionage.Engine.Resources
{
	public interface ITextureProvider : IResource
	{
		// Loading Meta
		float Progress { get; }
	}
}
