namespace Espionage.Engine
{
	/// <summary>
	/// Options have a UI representing them in a settings
	/// menu. use Library.Global.Properties.Select() to
	/// get them all.
	/// </summary>
	public class OptionAttribute : CookieAttribute
	{
		public OptionAttribute( string path = "config://cookies.ini" ) : base( path ) { }
	}
}
