using System;

namespace Espionage.Engine.Services
{
	[Group( "Services" )]
	public interface IService : ILibrary
	{
		/// <summary> Time it took to complete update loop </summary>
		public float Time { get; set; }

		void OnReady();
		void OnShutdown();

		void OnUpdate();
		void OnPostUpdate();
	}
}
