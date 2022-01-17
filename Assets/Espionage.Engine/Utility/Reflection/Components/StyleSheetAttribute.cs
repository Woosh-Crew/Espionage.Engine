using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class StyleSheetAttribute : Attribute, Library.IComponent
	{
		private readonly string _path;

		public StyleSheetAttribute( string path )
		{
			_path = path;
		}

#if UNITY_EDITOR
		public StyleSheet Style => AssetDatabase.LoadAssetAtPath<StyleSheet>( _path );
#else
		public StyleSheet Style => throw new NotImplementedException();
#endif
		public void OnAttached( ref Library library ) { }
	}
}
