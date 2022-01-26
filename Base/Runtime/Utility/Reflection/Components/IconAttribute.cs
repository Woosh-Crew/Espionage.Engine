using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Property )]
	public sealed class IconAttribute : Attribute, Library.IComponent, Property.IComponent
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
		public void OnAttached( ref Library library ) { }
		public void OnAttached( ref Property property ) { }
	}
}
