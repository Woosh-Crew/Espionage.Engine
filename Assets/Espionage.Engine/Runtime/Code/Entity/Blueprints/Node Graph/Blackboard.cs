using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Espionage.Engine.Entities
{
	public class Blackboard : ScriptableObject
	{
		[Serializable]
		public class Property
		{
			public string name;
			public object value;
			public Type type;

			public PropertyInfo info;
		}

		public List<Property> variables;

		public object GetProperty( string name )
		{
			return variables.FirstOrDefault( e => e.name == name );
		}
	}
}
