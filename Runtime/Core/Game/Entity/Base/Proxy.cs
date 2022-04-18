using UnityEngine;

namespace Espionage.Engine
{
	[SelectionBase]
	public class Proxy : Behaviour
	{
		public Entity Create()
		{
			var ent = Library.Create( className ) as Entity;

			if ( ent != null )
			{
				ent.Name = name;
			}

			return ent;
		}

		// Fields

		[SerializeField]
		internal bool disabled;
		
		[SerializeField]
		internal new string name;

		[SerializeField]
		internal  string className;
	}
}
