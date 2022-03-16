using System;
using System.IO;
using Espionage.Engine.Components;
using UnityEngine;
using Texture = Espionage.Engine.Resources.Texture;

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

		public Texture2D Icon
		{
			get
			{
				var texture = Texture.Find( Path );
				texture.Load();

				return texture.Provider.Texture;
			}
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
