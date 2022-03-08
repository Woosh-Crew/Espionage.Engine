using System;

namespace Espionage.Engine
{
	/// <summary>
	/// A PrefVar is just a cookie that
	/// has a UI element representing it.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = false )]
	public class PrefVar : CookieAttribute { }
}
