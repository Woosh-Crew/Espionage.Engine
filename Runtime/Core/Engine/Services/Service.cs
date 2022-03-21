namespace Espionage.Engine.Services
{
	[Group( "Services" )]
	public abstract partial class Service : IService
	{
		public Library ClassInfo { get; }

		public Service()
		{
			ClassInfo = Library.Register( this );
		}

		// Service

		public float Time { get; set; }

		public virtual void OnReady() { }

		public virtual void OnUpdate() { }
		public virtual void OnPostUpdate() { }

		public virtual void OnShutdown() { }

		public virtual void Dispose()
		{
			OnShutdown();
		}
	}
}
