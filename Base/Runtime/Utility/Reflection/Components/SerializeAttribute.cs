using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class SerializeAttribute : Attribute, Property.IComponent
	{
		private readonly bool _serialize;

		public SerializeAttribute( bool serialize = true )
		{
			_serialize = serialize;
		}

		public void OnAttached( ref Property property )
		{
			property.Serialized = _serialize;
		}
	}
}
