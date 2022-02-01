using UnityEngine.UIElements;

namespace Espionage.Engine.Editor
{
	public class Element : VisualElement, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		public Element()
		{
			ClassInfo = Library.Database.Get( GetType() );
			Callback.Register( this );

			foreach ( var item in ClassInfo.Components.GetAll<StyleSheetAttribute>() )
			{
				styleSheets.Add( item.Style );
			}
		}

		~Element()
		{
			Callback.Unregister( this );
		}
	}
}
