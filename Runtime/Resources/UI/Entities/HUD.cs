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

			Document = GetComponent<UIDocument>();

			// Setup UI Document
			Document.panelSettings = Res.Load<PanelSettings>( "UI Toolkit/PanelSettings" );

			var elements = CreateGUI( Document.visualTreeAsset );
			Document.rootVisualElement.Add( elements );
		}


		public virtual VisualElement CreateGUI( VisualTreeAsset root )
		{
			return null;
		}
	}
}
