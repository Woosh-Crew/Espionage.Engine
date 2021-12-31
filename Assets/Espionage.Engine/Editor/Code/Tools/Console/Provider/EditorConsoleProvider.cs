namespace Espionage.Engine.Internal
{
	public class EditorConsoleProvider : RuntimeConsoleProvider
	{
		public EditorConsoleProvider() : base( new AttributeCommandProvider<Console.CmdAttribute>() ) { }
	}
}
