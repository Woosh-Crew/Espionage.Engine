using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class CompanyAttribute : Attribute, IComponent<Library>, IComponent<Property>
	{
		public string Company { get; }

		public CompanyAttribute( string company )
		{
			Company = company;
		}

		public void OnAttached( Library library ) { }

		public void OnAttached( Property property ) { }
	}
}
