using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public interface IModelProvider : IResource, IAsset
	{
		GameObject Model { get; }
		float Progress { get; }
	}
}
