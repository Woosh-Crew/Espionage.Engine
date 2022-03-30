using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class Model : Resource<GameObject>
	{
		public static Model Load( string path, Action<GameObject> onLoad )
		{
			return null;
		}

		protected override void Load( Action<GameObject> onLoad ) { }

		protected override void Unload() { }
	}
}
