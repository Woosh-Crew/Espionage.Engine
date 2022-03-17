using UnityEngine;
using UnityEngine.UIElements;
using Res = UnityEngine.Resources;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "HUD" )]
	public class HUD : Entity
	{
		protected UI UI { get; private set; }

		protected override void OnAwake()
		{
			base.OnAwake();

			// Load UI XML first, if we have it
			if ( ClassInfo.Components.TryGet<FileAttribute>( out var file ) )
			{
				UI = UI.Find( file.Path );
				UI?.Load( e => Instantiate( e ) );

				return;
			}

			CreateGUI();
		}

		protected virtual void CreateGUI() { }
	}
}
