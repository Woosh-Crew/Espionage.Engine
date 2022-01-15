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
		// Blueprint Tree
		//

		public BehaviourTree Tree => _tree;

		[SerializeField]
		private BehaviourTree _tree;

		public void CreateTree()
		{
			// Node tree stores graph information

			_tree = ScriptableObject.CreateInstance<BehaviourTree>();
			_tree.name = "Node Tree";

#if UNITY_EDITOR
			AssetDatabase.AddObjectToAsset( _tree, this );
			AssetDatabase.SaveAssets();
#endif

		}

		public void CreateBlackboard()
		{
			// Blackboard holds variables
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
