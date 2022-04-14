using UnityEngine;

namespace Espionage.Engine.Gamemodes
{
	public class Team : Component
	{
		public virtual string Name => ClassInfo.Title;
		public virtual string Description { get; }
		public virtual Color Color => Color.white;
	}
}
