using UnityEngine;
using UnityEngine.UIElements;
using Res = UnityEngine.Resources;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "HUD" ), RequireComponent( typeof( UIDocument ) )]
	public class HUD : Entity
	{
		protected UI UI { get; private set; }
		protected UIDocument Document { get; private set; }

		protected override void OnAwake()
		{
			base.OnAwake();

			// Setup UI Document
			Document = GetComponent<UIDocument>();
			Document.panelSettings = Res.Load<PanelSettings>( "UI Toolkit/PanelSettings" );

			// Load UI XML first, if we have it
			if ( ClassInfo.Components.TryGet<FileAttribute>( out var file ) )
			{
				// Load UI
				UI = UI.Find( file.Path );
				UI.Load( e =>
				{
					Document.visualTreeAsset = e;
					CreateGUI();
				} );

				return;
			}

			CreateGUI();
		}

		protected virtual void CreateGUI() { }
	}
}
