using UnityEngine.UIElements;

namespace Espionage.Engine
{
	public static class VisualElementExtensions
	{
		public static T Add<T>( this VisualElement element, string name = null, params string[] classes ) where T : VisualElement, new()
		{
			var newElement = new T();
			newElement.name = name;

			foreach ( var item in classes )
				newElement.AddToClassList( item );

			element.Add( newElement );
			return newElement;
		}
	}
}
