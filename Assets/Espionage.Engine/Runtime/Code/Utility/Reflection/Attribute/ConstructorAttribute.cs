using System;

namespace Espionage.Engine.Internal
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class ConstructorAttribute : Attribute
	{
		public ConstructorAttribute( string constructor )
		{
			this.constructor = constructor;
		}

		private string constructor;
		public string Constructor => constructor;
	}
}
