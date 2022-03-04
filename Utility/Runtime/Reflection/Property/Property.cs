using System;
using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public sealed class Property : IMember
	{
		public Library Owner { get; }

		public PropertyInfo Info { get; }
		public Components<Property> Components { get; }

		internal Property( Library owner, PropertyInfo info )
		{
			Owner = owner;
			Info = info;

			Name = info.Name;
			Title = info.Name;
			Group = owner.Title;

			// Components
			Components = new Components<Property>( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			foreach ( var item in Info.GetCustomAttributes() )
			{
				if ( item is IComponent<Property> property )
				{
					Components.Add( property );
				}
			}
		}

		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Serialized { get; set; }

		// Helpers

		public bool IsStatic => Info.GetMethod.IsStatic;
		public Type Type => Info.PropertyType;

		// Values

		public object Default => Info.GetConstantValue();

		public object this[ object from ]
		{
			get => Info.GetValue( from );
			set => Info.SetValue( from, value );
		}
	}
}
