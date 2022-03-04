using System;
using Espionage.Engine.Components;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	/// <summary>
	/// Reflection Component that stores a reference to an Icon.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method )]
	public sealed class IconAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		public string Path { get; }

		public IconAttribute( string path )
		{
			Path = path;
		}

	#if UNITY_EDITOR
		public Texture Icon => AssetDatabase.LoadAssetAtPath<Texture>( Path );
	#else
		public Texture Icon => throw new NotImplementedException();
	#endif

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
