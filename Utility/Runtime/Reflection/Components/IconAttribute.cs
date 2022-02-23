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
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class IconAttribute : Attribute, IComponent<Library>, IComponent<Property>, IComponent<Function>
	{
		private readonly string _path;

		public IconAttribute( string path )
		{
			_path = path;
		}

	#if UNITY_EDITOR
		public Texture Icon => AssetDatabase.LoadAssetAtPath<Texture>( _path );
	#else
		public Texture Icon => throw new NotImplementedException();
	#endif
		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
