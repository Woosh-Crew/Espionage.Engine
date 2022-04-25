using System.Collections.Generic;
using Espionage.Engine.IO;

namespace Espionage.Engine.Resources
{
	public sealed class Resource
	{
		public bool Persistant { get; set; }
		public bool IsLoaded => Source != null;

		public IAsset Source { get; set; }
		public Stack<IAsset> Instances { get; } = new();

		public int Identifier { get; }
		public Pathing Path { get; }

		public Resource( Pathing path )
		{
			Path = path;
			Identifier = path.Hash();
		}

		public T Create<T>() where T : class, IAsset, new()
		{
			Assert.IsTrue( Source != null );

			Source = new T();
			Source.Resource = this;
			Source.Setup( Path );

			return Source as T;
		}
	}
}
