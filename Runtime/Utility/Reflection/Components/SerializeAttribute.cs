using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that says whether or not this property can be serialized. 
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class SerializeAttribute : Attribute, IComponent<Property>
	{
		private readonly bool _serialize;

		public SerializeAttribute( bool serialize = true )
		{
			_serialize = serialize;
		}

		public void OnAttached( Property property )
		{
			property.Serialized = _serialize;
		}
	}
}
