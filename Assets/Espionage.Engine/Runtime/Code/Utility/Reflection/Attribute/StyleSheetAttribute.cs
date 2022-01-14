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
		public Library Library { get; set; }

		public StyleSheetAttribute( string path )
		{
			_path = path;
		}

		private string _path;

		public StyleSheet Style => AssetDatabase.LoadAssetAtPath<StyleSheet>( _path );
	}
}
