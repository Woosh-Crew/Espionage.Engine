using UnityEngine;

namespace Espionage.Engine
{
	public class Proxy : Entity
	{
		protected override void OnAwake()
		{
			base.OnAwake();

			if ( Library.Database.TryGet( target, out var lib ) )
			{
				// Create Entity, based off proxy
				Library.Create( lib );
			}
		}

		// Fields

		[SerializeField]
		private string target;
	}
}
