using System;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class CompanyAttribute : Attribute, Library.IComponent, Property.IComponent
	{
		public string Company { get; }

		public CompanyAttribute( string company )
		{
			Company = company;
		}

		public void OnAttached( ref Library library ) { }

		public void OnAttached( ref Property property ) { }
	}
}
