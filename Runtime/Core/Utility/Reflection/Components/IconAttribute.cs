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
		public string Path { get; set; }
		public string GUID { get; set; }

	#if UNITY_EDITOR
		public Texture2D Icon => AssetDatabase.LoadAssetAtPath<Texture2D>( !string.IsNullOrEmpty( GUID ) ? AssetDatabase.GUIDToAssetPath( GUID ) : Path );
	#else
		public Texture2D Icon => throw new NotImplementedException();
	#endif

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
