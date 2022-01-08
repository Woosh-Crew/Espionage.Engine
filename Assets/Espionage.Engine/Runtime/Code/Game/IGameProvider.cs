using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine
{
	public interface IGameProvider
	{
		uint AppId { get; }

		void Ready();
		void Shutdown();
		void OnLevelLoaded(/* Probably pass through a level */);
	}
}
