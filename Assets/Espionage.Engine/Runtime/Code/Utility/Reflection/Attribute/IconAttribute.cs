
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	public sealed class IconAttribute : Library.Component
	{
		public IconAttribute( string path )
		{
			_path = path;
		}

		private string _path;

		public Texture Icon { get; private set; }

		public override void OnAttached()
		{
#if UNITY_EDITOR
			Icon = AssetDatabase.LoadAssetAtPath<Texture>( _path );
#endif
		}
	}
}
