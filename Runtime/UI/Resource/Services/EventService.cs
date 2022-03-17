using Espionage.Engine.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Espionage.Engine.Resources.Services
{
	internal class EventService : Service
	{
		public EventSystem System { get; private set; }
		
		public override void OnReady()
		{
			System = EventSystem.current ? EventSystem.current : new GameObject("Event System").AddComponent<EventSystem>();
			System.gameObject.MoveTo( Engine.Scene );
		}
	}
}
