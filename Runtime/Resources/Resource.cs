using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public sealed class Resource
	{
		public bool IsLoaded => Asset != null;

		public IAsset Asset { get; set; }
		public bool Persistant { get; set; }

		public int Identifier { get; }
		public Pathing Path { get; }

		public Resource( Pathing path )
		{
			Path = path;
			Identifier = path.Hash();
		}

		public T Create<T>() where T : class, IAsset, new()
		{
			Assert.IsTrue( Asset != null );

			Asset = new T();
			Asset.Resource = this;
			Asset.Setup( Path );

			return Asset as T;
		}
	}
}
