using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component for storing data about file specific meta.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class FileAttribute : Attribute, IComponent<Library>
	{
		/// <summary>
		/// The Default name of this file.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The default extension for this file.
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		/// The serialization type for this file.
		/// </summary>
		public string Serialization { get; set; }

		public void OnAttached( Library library ) { }
	}
}
