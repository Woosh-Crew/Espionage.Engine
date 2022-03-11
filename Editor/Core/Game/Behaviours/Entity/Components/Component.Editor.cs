using Espionage.Engine.Internal;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Component ), true )]
	internal class ComponentEditor : BehaviourEditor
	{
		protected virtual void OnEnable()
		{
			base.OnEnable();
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title} (Component)";
		}
	}
}
