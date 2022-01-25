using System;
using System.Reflection;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property )]
	public class PreferenceAttribute : Attribute, Property.IComponent
	{
		private string _name;
		private string _title;
		private string _tooltip;
		private string _group;

		public Preferences.Item Create( PropertyInfo info )
		{
			return new Preferences.Item()
			{
				Name = _name,
				Title = _title,
				Tooltip = _tooltip,
				Group = _group
			};
		}

		public void OnAttached( ref Property property )
		{
			_name = property.Name;
			_title = property.Title;
			_tooltip = property.Help;
			_group = property.Group;
		}
	}
}
