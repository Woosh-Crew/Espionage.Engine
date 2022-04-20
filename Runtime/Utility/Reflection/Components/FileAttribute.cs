using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component for storing data about file specific meta.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, Inherited = false )]
	public sealed class FileAttribute : Attribute, IComponent<Library>
	{
		/// <summary>
		/// What file should we load by default if loading one failed.
		/// </summary>
		public string Fallback { get; set; }

		/// <summary>
		/// The default extension for this file.
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		/// The serialization type for this file.
		/// </summary>
		public Serialization Serialization { get; set; }

		public void OnAttached( Library library ) { }
	}

	public enum Serialization { Json, Binary, Ini, Yaml, XML }
}
