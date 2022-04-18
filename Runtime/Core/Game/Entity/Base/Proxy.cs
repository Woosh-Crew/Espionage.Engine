using UnityEngine;

namespace Espionage.Engine
{
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
		private new string name;

		[SerializeField]
		private string className;
	}
}
