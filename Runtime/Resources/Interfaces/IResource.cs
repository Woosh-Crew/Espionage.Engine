using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public interface IResource<out T> : IResource
	{
		void Load( Action<T> loaded = null );
		void Unload( Action unloaded = null );
	}

	/// <summary>
	/// A Resource is a reference to an asset
	/// </summary>
	public interface IResource
	{
		string Identifier { get; }
	}
}
