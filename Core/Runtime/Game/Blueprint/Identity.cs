using UnityEngine;

namespace Espionage.Engine
{
	[AddComponentMenu( "" ), Editable( false )]
	public class Identity : Behaviour
	{
		public Library Library { get; internal set; }
	}
}
