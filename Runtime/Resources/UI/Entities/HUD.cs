using UnityEngine;
using UnityEngine.UIElements;
using Res = UnityEngine.Resources;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "HUD" ), RequireComponent( typeof( UIDocument ) )]
	public class HUD : Entity
	{
		public UI UI { get; private set; }
		public UIDocument Document { get; private set; }

		protected override void OnAwake()
		{
			base.OnAwake();

			// Setup UI Document
			Document = GetComponent<UIDocument>();
			Document.panelSettings = Res.Load<PanelSettings>( "UI Toolkit/PanelSettings" );

			if ( ClassInfo.Components.TryGet<FileAttribute>( out var file ) )
			{
				// Load UI
				UI = UI.Find( file.Path );
				UI.Load( e =>
				{
					Document.visualTreeAsset = e;
					CreateGUI();
				} );
			}
		}

		protected virtual void CreateGUI() { }
	}
}
