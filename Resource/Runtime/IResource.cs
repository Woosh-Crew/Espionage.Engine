using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public interface IResource
	{
		string Path { get; }
		bool IsLoading { get; }

		void Load( Action onLoad = null );
		void Unload( Action onUnload = null );
	}
}
