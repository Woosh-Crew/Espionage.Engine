using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine.Entities
{
	[Library.Skip, CreateAssetMenu( menuName = "Espionage.Engine/Blueprint", fileName = "Blueprint" )]
	public class Blueprint : ScriptableObject, ILibrary
	{
		[field: SerializeField]
		public Library ClassInfo { get; set; }

		private static ILibrary Constructor( Library library )
		{
			return null;
		}

		//
		// Editor
		//

#if UNITY_EDITOR

		public VisualElement InspectorUI()
		{
			var root = new VisualElement();

			OnInspectorUI( ref root );

			return root;
		}

		protected virtual void OnInspectorUI( ref VisualElement root )
		{

		}

#endif
	}
}
