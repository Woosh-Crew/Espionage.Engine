using System.Text;

namespace Espionage.Engine.Serializers
{
	[Library, Group( "Serializers" )]
	internal class StringSerializer : ISerializer<string>
	{
		public byte[] Serialize( string item )
		{
			return Files.UTF8.GetBytes( item );
		}
	}
}
