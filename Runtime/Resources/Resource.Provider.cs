using System;

namespace Espionage.Engine.Resources
{
	public abstract partial class Resource
	{
		public interface IProvider<T, out TOut> where T : IResource
		{
			float Progress { get; }
			string Identifier { get; }

			void Load( Action<TOut> onLoad = null );
			void Unload( Action onUnload = null );
		}

		public interface IProvider<T> where T : IResource
		{
			float Progress { get; }
			string Identifier { get; }
		}
	}
}
