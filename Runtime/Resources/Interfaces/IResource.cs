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
		bool IsLoading { get; }

		void Load( Action onLoad = null );
		void Unload( Action onUnload = null );
	}
}
