using UnityEngine.UIElements;

namespace Espionage.Engine.Editor
{
	[Group( "UI Elements" )]
	public class Element : VisualElement, ILibrary
	{
		public Library ClassInfo { get; set; }

		public Element()
		{
			ClassInfo = Library.Register( this );

			foreach ( var item in ClassInfo.Components.GetAll<StyleSheetAttribute>() )
			{
				styleSheets.Add( item.Style );
			}
		}

		~Element()
		{
			Library.Unregister( this );
		}
	}
}
