using UnityEngine;

namespace Espionage.Engine.Entities
{
	public class PrintNode : Node
	{
		protected override bool OnExecute()
		{
			Debug.Log( "Executing" );
			return true;
		}
	}
}
