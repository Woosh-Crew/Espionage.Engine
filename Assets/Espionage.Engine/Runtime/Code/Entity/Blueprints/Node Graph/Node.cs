using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Entities
{
	public enum State { Running, Failure, Success }

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

		public string id;
		public Vector2 position;

		//
		// Inputs and Outputs
		//

		public void BuildInputs() { }
		public void BuildOutputs() { }

		protected virtual void OnBuildInputs() { }
		protected virtual void OnBuildOutputs() { }
	}
}
