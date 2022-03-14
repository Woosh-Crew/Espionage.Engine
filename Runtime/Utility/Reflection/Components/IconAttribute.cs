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

		public Texture2D Icon
		{
			get
			{
				// BUG : Memory leak right here!
				// TODO : Fix this after we implement texture resources.

				var image = Files.Serialization.Deserialize( Path );

				var texture = new Texture2D( 2, 2 );
				texture.LoadImage( image );

				return texture;
			}
		}

		public void OnAttached( Library library ) { }
		public void OnAttached( Property property ) { }
		public void OnAttached( Function item ) { }
	}
}
