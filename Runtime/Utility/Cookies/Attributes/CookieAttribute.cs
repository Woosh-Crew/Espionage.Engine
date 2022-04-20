using System;
using Espionage.Engine.Components;
using Espionage.Engine.Internal;

namespace Espionage.Engine
{
	/// <summary>
	/// A Cookie is a static property where its state gets saved
	/// globally, it gets saved to a simple config file.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = false )]
	public class CookieAttribute : Attribute, IComponent<Property>
	{
		public string Path { get; }

		public CookieAttribute( string path = "config://cookies.ini" )
		{
			Path = path;
		}


		public void OnAttached( Property item )
		{
			Assert.IsFalse( item.IsStatic, $"Property {item.Name} from {item.Owner?.Name} can't be instanced" );
			Cookies.Register( new( Path, item ) );
		}
	}
}
