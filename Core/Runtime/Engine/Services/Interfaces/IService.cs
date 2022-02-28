using System;

namespace Espionage.Engine.Services
{
	[Group( "Services" )]
	public interface IService : ILibrary, IDisposable
	{
		void OnReady();
		void OnShutdown();
		void OnUpdate();
	}
}
