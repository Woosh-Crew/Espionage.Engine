using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	/// <summary>
	/// A Resource is a reference to an asset
	/// </summary>
	public interface IResource
	{
		string Identifier { get; }

		void Load( Action loaded = null );
		void Unload( Action unloaded = null );
	}
}
