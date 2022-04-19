namespace Espionage.Engine
{
	public class Common : ILibrary
	{
		public Library ClassInfo { get; }

		public Common()
		{
			ClassInfo = Library.Register( this );
		}

		public virtual void Delete()
		{
			Library.Unregister( this );
		}
	}
}
