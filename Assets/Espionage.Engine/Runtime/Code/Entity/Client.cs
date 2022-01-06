using Steamworks;

namespace Espionage.Engine
{
	public class Client : Entity
	{
		public string Username { get; private set; }
		public ulong Identifier { get; set; }
	}
}
