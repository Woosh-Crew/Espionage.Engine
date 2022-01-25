using System;
using System.Reflection;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property )]
	public class PreferenceAttribute : Attribute, Property.IComponent
	{
		private Property _property;

		public void OnAttached( ref Property property )
		{
			_property = property;
		}
	}
}
