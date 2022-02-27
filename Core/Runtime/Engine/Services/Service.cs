namespace Espionage.Engine.Services
{
	public abstract class Service : IService
	{
		public Library ClassInfo { get; }

		public Service()
		{
			ClassInfo = Library.Register( this );
		}

		public virtual void OnReady() { }
		public virtual void OnUpdate() { }
		public virtual void OnShutdown() { }

		public virtual void Dispose()
		{
			OnShutdown();
		}
	}
}
