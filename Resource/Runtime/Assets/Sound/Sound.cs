using System;

namespace Espionage.Engine.Resources
{
	public class Sound : IResource
	{
		public string Path { get; }
		public bool IsLoading { get; }

		public bool Load( Action onLoad = null )
		{
			throw new NotImplementedException();
		}

		public bool Unload( Action onUnload = null )
		{
			throw new NotImplementedException();
		}
	}
}
