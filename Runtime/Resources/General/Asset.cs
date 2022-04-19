namespace Espionage.Engine.Resources
{
	[Group( "Data" ), Path( "custom_assets", "assets://Data", Overridable = true )]
	public abstract class Asset : IResource
	{
		public Library ClassInfo { get; }

		public Asset()
		{
			ClassInfo = Library.Register( this );
		}

		// Resource

		int IResource.Identifier { get; set; }
		bool IResource.Persistant { get; set; }

		void IResource.Setup( string path )
		{
			Source = path;
		}

		protected string Source { get; private set; }

		void IResource.Load()
		{
			OnLoad();
		}

		protected virtual void OnLoad() { }

		bool IResource.Unload()
		{
			OnUnload();

			return false;
		}

		protected virtual void OnUnload() { }
	}
}
