using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public interface ITextureProvider : IResource, IAsset
	{
		public Texture2D Texture { get; }
		float Progress { get; }
	}
}
