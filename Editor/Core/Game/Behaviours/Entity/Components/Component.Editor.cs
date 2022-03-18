using Espionage.Engine.Internal;
using UnityEditor;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Component ), true )]
	internal class ComponentEditor : BehaviourEditor
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}  <size=10>- Component</size>";

			var type = ClassInfo.Class;
			var isGeneric = false;

			while ( type != typeof( Component ) )
			{
				if ( type.IsGenericType && type.GetGenericTypeDefinition() == typeof( Component<> ) )
				{
					isGeneric = true;
					break;
				}

				type = type.BaseType;
			}

			if ( !isGeneric )
			{
				return;
			}

			var required = Library.Database[type.GetGenericArguments()[0]];

			// Check if we actually can add this component
			if ( !((Component)target).TryGetComponent( required.Class, out _ ) )
			{
				EditorUtility.DisplayDialog(
					$"Missing required Entity ({required.Title})",
					$"Can't add Component {ClassInfo.Title} to this GameObject. missing required Entity \"{required.Title}\"",
					"Okay" );

				DestroyImmediate( target );
			}

			// Apply title, if all is good
			var inherited = required.Title;
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title}  <size=10>- Component for {inherited}</size>";
		}
	}
}
