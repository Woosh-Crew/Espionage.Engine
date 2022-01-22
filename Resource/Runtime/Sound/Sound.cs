using System;

namespace Espionage.Engine.Resources
{
	public class Sound : IResource
	{
		public string Path { get; }
		public bool IsLoading { get; }
		
		public void Load( Action onLoad = null )
		{
			throw new NotImplementedException();
		}

		public void Unload( Action onUnload = null )
		{
			throw new NotImplementedException();
		}
	}
}
