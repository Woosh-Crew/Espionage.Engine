using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	public enum State { Running, Failure, Success }

	public abstract class Node : ScriptableObject, ILibrary, ICallbacks
	{
		//
		// Init
		//

		public Library ClassInfo { get; set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		//
		// Graph
		//

		private Vector2 _position;

		//
		// Inputs and Outputs
		//

		public void BuildInputs() { }
		public void BuildOutputs() { }

		protected virtual void OnBuildInputs() { }
		protected virtual void OnBuildOutputs() { }
	}
}
