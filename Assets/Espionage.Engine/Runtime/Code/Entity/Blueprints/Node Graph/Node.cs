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

		/// <summary> Run this node, and then executes all child nodes </summary>
		/// <returns> Return true if execution was successful </returns>
		public bool Execute()
		{
			// Execute all inputs that are not methods....
			// Then execute the value type inputs

			return OnExecute();
		}

		protected virtual bool OnExecute()
		{
			return true;
		}

		//
		// Inputs & Outputs
		//

		public Input[] Inputs { get; set; }
		public Output[] Outputs { get; set; }

		public class Port
		{
			public bool AllowMultiple { get; private set; }

			public Type Type => _type;
			private Type _type;
		}

		public class Input : Port
		{
			public IReadOnlyList<Output> Outputs => _outputs;
			private List<Output> _outputs;

			// Database
			public void Add( Output output )
			{
				if ( !output.Contains( this ) )
				{
					Debugging.Log.Error( "Illegal Add to Input Node Port" );
					return;
				}

				_outputs.Add( output );
			}

			public void Remove( Output output )
			{
				if ( !output.Contains( this ) )
				{
					Debugging.Log.Error( "Illegal Remove to Input Node Port" );
					return;
				}

				_outputs.Remove( output );
			}
		}

		public class Output : Port
		{
			public IReadOnlyList<Input> Inputs => _targets;
			private List<Input> _targets;

			// Database
			public void Add( Input input )
			{
				if ( input.Type != Type )
				{
					Debugging.Log.Warning( "Invalid Port 2 Port Type" );
					return;
				}

				_targets.Add( input );
				input.Add( this );
			}

			public void Remove( Input input )
			{
				input.Remove( this );
				_targets.Remove( input );
			}

			public bool Contains( Input input )
			{
				return _targets.Contains( input );
			}
		}

		[AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = true )]
		protected class InputAttribute : Attribute { }

		[AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = true )]
		protected class OutputAttribute : Attribute { }
	}
}
