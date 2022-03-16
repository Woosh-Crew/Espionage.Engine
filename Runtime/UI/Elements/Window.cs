using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Espionage.Engine.UI	
{
	public class Window : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Window, UxmlTraits> {}
		public new class UxmlTraits : VisualElement.UxmlTraits { }

		public Window()
		{
		}
	}
}
