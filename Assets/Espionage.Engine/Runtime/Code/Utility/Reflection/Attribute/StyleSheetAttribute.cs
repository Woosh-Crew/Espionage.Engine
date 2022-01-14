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
		public StyleSheetAttribute( string path )
		{
			_path = path;
		}

		private string _path;

		public StyleSheet Style { get; private set; }

		//
		// Component
		//

		public Library Library { get; set; }

		public void OnAttached()
		{
#if UNITY_EDITOR
			Style = AssetDatabase.LoadAssetAtPath<StyleSheet>( _path );
#endif
		}
	}
}
