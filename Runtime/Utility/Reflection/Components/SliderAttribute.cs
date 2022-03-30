using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage(AttributeTargets.Property)]
	public class SliderAttribute : Attribute, IComponent<Property>
	{
		public float Min { get; }
		public float Max { get; }

		public SliderAttribute( float min, float max )
		{
			Min = min;
			Max = max;
		}
		
		public void OnAttached( Property item ) { }
	}
}
