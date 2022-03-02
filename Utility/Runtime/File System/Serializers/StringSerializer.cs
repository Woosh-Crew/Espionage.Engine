using System;
using System.Text;

namespace Espionage.Engine.Serializers
{
	[Group( "Serializers" )]
	internal class StringSerializer : ISerializer<char>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( StringSerializer )];

		public byte[] Serialize( char[] item )
		{
			return Files.UTF8.GetBytes( item );
		}
	}
}
