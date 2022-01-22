using System;
using UnityEngine;

namespace Espionage.Engine.Assets
{
	public interface IResource
	{
		string Path { get; }
		bool IsLoading { get; }
		AssetBundle Bundle { get; }
		
		
		void Load( Action onLoad = null  );
		void Unload( Action onUnload = null );
	}
}
