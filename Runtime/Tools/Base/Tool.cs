namespace Espionage.Engine.Tools
{
	public abstract class Tool : ILibrary
	{
		public Library ClassInfo { get; }

		public Tool()
		{
			ClassInfo = Library.Register( this );
		}

		public void Delete()
		{
			Library.Unregister( this );
		}
	}
}
