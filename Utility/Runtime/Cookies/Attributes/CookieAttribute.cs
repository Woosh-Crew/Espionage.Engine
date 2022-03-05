using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// A Cookie is a static property where its state gets saved
	/// globally, it gets saved to a simple config file.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = false )]
	public class CookieAttribute : Attribute, IComponent<Property>
	{
		public void OnAttached( Property item )
		{
			if ( !item.IsStatic )
			{
				throw new InvalidOperationException( $"Property {item.Name} from {item.Owner.Name} can't be instanced" );
			}

			Debugging.Log.Info( item.Name );
		}
	}
}
