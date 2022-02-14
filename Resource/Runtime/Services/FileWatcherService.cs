using Espionage.Engine.Services;

namespace Espionage.Engine.Resources
{
	public class FileWatcherService : IService
	{
		public Library ClassInfo { get; }

		public FileWatcherService()
		{
			ClassInfo = Library.Database[GetType()];
		}
		
		public void OnReady() {  }
		public void OnUpdate() { }
		public void OnShutdown() {  }
		public void Dispose() {  }
	}
}
