using System.IO;
using Espionage.Engine.Services;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class MapService : IService
	{
		public Library ClassInfo { get; }

		public MapService()
		{
			ClassInfo = Library.Database[GetType()];
		}

		//
		// Service
		//

		public void OnReady() { }

		public void OnShutdown() { }

		public void OnUpdate() { }
	}
}
