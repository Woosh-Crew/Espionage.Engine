using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A Resource is a reference to an asset
	/// </summary>
	public interface IResource
	{
		string Path { get; }
		bool IsLoading { get; }

		bool Load( Action onLoad = null );
		bool Unload( Action onUnload = null );
	}
}
