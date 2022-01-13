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
		// Attributes
		//

		[AttributeUsage( AttributeTargets.Property | AttributeTargets.Method, Inherited = true )]
		protected sealed class InputAttribute : Attribute { }

		[AttributeUsage( AttributeTargets.Property | AttributeTargets.Method, Inherited = true )]
		protected sealed class OutputAttribute : Attribute { }


		//
		// Init
		//

		public Library ClassInfo { get; set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}
	}
}
