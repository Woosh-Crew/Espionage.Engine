using System;
using System.Text;

namespace Espionage.Engine.Serializers
{
	[Group( "Serializers" )]
	internal class StringSerializer : ISerializer<char>, ISerializer<string>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( StringSerializer )];

		// Char

		public byte[] Serialize( char item )
		{
			throw new InvalidOperationException( "Why?" );
		}

		public byte[] Serialize( char[] item )
		{
			return Files.UTF8.GetBytes( item );
		}

		// String

		public byte[] Serialize( string item )
		{
			return Files.UTF8.GetBytes( item );
		}

		public byte[] Serialize( string[] item )
		{
			throw new NotImplementedException();
		}
	}
}
