using System;
using UnityEngine;

namespace Espionage.Engine.Resources.Provider
{
	public class AssetBundleModelProvider : Resource.IProvider<Model, GameObject>
	{
		public GameObject Output { get; }
		public float Progress { get; }
		public string Identifier { get; }
		public bool IsLoading { get; }
		
		public void Load( Action onLoad = null ) {  }
		public void Unload( Action onUnload = null ) {  }
	}
}
