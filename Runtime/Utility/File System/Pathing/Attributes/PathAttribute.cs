using System;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
	public class PathAttribute : Attribute, IComponent<Library>
	{
		public string ShortHand { get; }
		public string Path { get; }
		
		public bool Overridable { get; set; }

		public PathAttribute( string shortHand, string path )
		{
			ShortHand = shortHand;
			Path = path;
		}

		public void OnAttached( Library item )
		{
			Files.Pathing.Add( ShortHand, Path );
		}
	}
}
