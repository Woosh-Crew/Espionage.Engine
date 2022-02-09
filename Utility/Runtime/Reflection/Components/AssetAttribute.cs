using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class )]
	public class AssetAttribute : Attribute, IComponent<Library>
	{
		/// <summary>
		/// The path to the asset.
		/// </summary>
		public string Path { get; set; }

		public void OnAttached( Library library ) { }
	}
}
