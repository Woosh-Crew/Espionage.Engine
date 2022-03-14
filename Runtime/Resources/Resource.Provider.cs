using System;

namespace Espionage.Engine.Resources
{
	public abstract partial class Resource
	{
		public interface IProvider<T> where T : IResource
		{
			float Progress { get; }
			string Identifier { get; }

			void Load( Action onLoad = null );
			void Unload( Action onUnload = null );
		}
	}
}
