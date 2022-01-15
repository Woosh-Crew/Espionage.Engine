using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	[Library.Constructor( nameof( Constructor ) )]
	public abstract class Node : ScriptableObject, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; set; }

		private void Awake()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		public static ILibrary Constructor( Library library )
		{
			return ScriptableObject.CreateInstance( library.Class ) as ILibrary;
		}

		//
		// Graph
		//

		public BehaviourTree Tree => _tree;
		internal BehaviourTree _tree;

		public Node child;

		/// <summary> Run this node, and then executes all child nodes </summary>
		/// <returns> Return true if execution was successful </returns>
		public bool Execute()
		{
			if ( OnExecute() )
			{
				return child?.Execute() ?? true;
			}

			return false;
		}

		protected virtual bool OnExecute()
		{
			return true;
		}

		//
		// Editor
		//

#if UNITY_EDITOR

		public string id;
		public Vector2 position;

#endif
	}
}
