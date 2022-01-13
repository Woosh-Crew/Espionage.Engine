using System;
using UnityEngine;
using UnityEngine.UIElements;

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

#if UNITY_EDITOR

		public VisualElement InspectorUI()
		{
			return null;
		}

#endif
	}
}
