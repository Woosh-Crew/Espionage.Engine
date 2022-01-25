using System;
using System.Reflection;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property )]
	public class PreferenceAttribute : Attribute
	{
		public string Title { get; set; }
		public string Tooltip { get; set; }

		public Preferences.Item Create( PropertyInfo info )
		{
			return new Preferences.Item()
			{
				Name = $"{info.DeclaringType?.Name}::{info.Name}::{info.PropertyType}",
				Title = Title,
				Tooltip = Tooltip
			};
		}
	}
}
