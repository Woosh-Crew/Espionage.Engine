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

		public float Time { get; set; }

		// State
		public virtual void OnReady() { }
		public virtual void OnShutdown() { }

		// Update
		public virtual void OnUpdate() { }
		public virtual void OnPostUpdate() { }
	}
}
