using UnityEngine;

namespace Espionage.Engine.Services
{
	public class DevService : Service
	{
		public override void OnUpdate()
		{
			// Use Dev Tripod
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Debugging.Console.Invoke( "dev.tripod" );
			}
			
			// Open Terminal
			if ( Input.GetKeyDown( KeyCode.F3 ) )
			{
				Debugging.Console.Invoke( "dev.noclip" );
			}
			
			// Open Terminal
			if ( Input.GetKeyDown( KeyCode.F2 ) )
			{
				Debugging.Console.Invoke( "dev.terminal" );
			}
		}
	}
}
