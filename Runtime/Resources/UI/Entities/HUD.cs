using UnityEngine;
using UnityEngine.UIElements;

using Res = UnityEngine.Resources;

namespace Espionage.Engine.Resources
{
	[RequireComponent( typeof( UIDocument ) )]
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
		}
		

		public void CreateGUI() { }
	}
}
