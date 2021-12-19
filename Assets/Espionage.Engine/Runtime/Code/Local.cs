namespace Espionage.Engine
{
	public static class Local
	{
		[Console.Var( "client", IsReadOnly = true )]
		public static Client Client { get; internal set; }
	}
}
