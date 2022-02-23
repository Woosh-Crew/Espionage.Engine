using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Components;
using UnityEngine.LowLevel;

namespace Espionage.Engine
{
	public sealed class Property
	{
		public Components<Property> Components { get; }

		internal Property( Library owner, PropertyInfo info )
		{
			ClassInfo = owner;
			Info = info;

			Name = info.Name;
			Title = info.Name;

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
		public Type Type => Info.PropertyType;

		private Library ClassInfo { get; }
		private PropertyInfo Info { get; }

		public object this[ object from ]
		{
			get => Info.GetValue( from );
			set => Info.SetValue( from, value );
		}
	}
}
