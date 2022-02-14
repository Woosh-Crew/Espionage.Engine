using System;

namespace Espionage.Engine.Services
{
	public interface IService : ILibrary, ICallbacks, IDisposable
	{
		void OnReady();
		void OnShutdown();
		void OnUpdate();
	}
}
