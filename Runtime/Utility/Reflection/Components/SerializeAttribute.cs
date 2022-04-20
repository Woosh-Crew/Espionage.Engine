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
		/// <summary>
		/// Overrides the property serialization identifier.
		/// Use this if you change the name of your property.
		/// </summary>
		public int Identifier { get; set; }

		public SerializeAttribute( bool serialize = true )
		{
			_serialize = serialize;
		}
		
		private readonly bool _serialize;

		public void OnAttached( Property property )
		{
			property.Serialized = _serialize;
		}
	}
}
