using System;
using System.IO;
using Espionage.Engine.Components;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Networking;
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

		#if UNITY_EDITOR

		public Texture2D Icon
		{
			get
			{
				var path = Files.Path( Path, "project://" );
				Debugging.Log.Info( path );
				return AssetDatabase.LoadAssetAtPath<Texture2D>( path );
			}
		}

		#endif

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
