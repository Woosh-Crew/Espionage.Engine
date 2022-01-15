using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class IconAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public IconAttribute( string path )
		{
			_path = path;
		}

		private string _path;

#if UNITY_EDITOR
		public Texture Icon => AssetDatabase.LoadAssetAtPath<Texture>( _path );
#else
		public Texture Icon => throw new NotImplementedException();
#endif
	}
}
