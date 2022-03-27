namespace Espionage.Engine.Tools
{
	public partial class Inspector
	{
		public abstract class Editor<T> : Editor where T : class, ILibrary
		{
			public new T Target => base.Target as T;
		}
		
		public abstract class Editor : ILibrary
		{
			public Library ClassInfo { get; }
			
			public Editor()
			{
				ClassInfo = Library.Register(this);				
			}
			
			public object Target { get; set; }

			public abstract void OnLayout();
		}
		
		//
		// Drawers 
		//

		[Singleton]
		public abstract class Drawer<T> : ILibrary where T : unmanaged
		{
			public Library ClassInfo => Library.Database[GetType()];
			public abstract void OnLayout( T item );
		}
	}
}
