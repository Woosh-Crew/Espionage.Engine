using Espionage.Engine;
using UnityEngine;

[Title( "Sneaky Killer" )]
public class SneakyKiller : Game
{
	public override void OnReady() { }
	public override void OnShutdown() { }

	protected override void PostControlSetup( Controls.Setup setup )
	{
		base.PostControlSetup( setup );

		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			setup.Cursor.Visible = !setup.Cursor.Visible;
			setup.Cursor.Locked = !setup.Cursor.Visible;
		}
	}
}
