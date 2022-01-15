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

		public bool Execute()
		{
			// Execute will be invoked once we've proceeded all inputs
			// This is the middle ground between inputs and outputs

			// So for example, hardcoding output variables to do certian logic
			// Based on input variables.

			// OnExecute() => outputVar = variableA + variableB

			// We have to make sure input methods call execute
			// Unless you don't have any outputs

			// After we've finished executing we will procede to the
			// child input and invoke the targeted input.

			// I need to think of a way to make this work with multiple input and
			// Output methods. Maybe we could put this logic on the port itself?
			// and just make the node the container / processor of ports

			// How would serialization work with the ports though? Really tough shit.
			// The only way I think we could solve this issue is not keep a reference to
			// Any output ports that are on out parents. But how would variable ports work then?
			// We could probably serialize them in a separate class.  


			if ( OnExecute() )
			{
			}

			return false;
		}

		protected virtual bool OnExecute()
		{
			return true;
		}

		//
		// Behaviour Tree
		//

		public BehaviourTree Tree => _tree;
		private BehaviourTree _tree;

		public virtual void OnAdded( BehaviourTree tree )
		{
			_tree = tree;
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
