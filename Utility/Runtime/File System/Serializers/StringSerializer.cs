using System;
using System.Text;

namespace Espionage.Engine.Serializers
{
	[Group( "Serializers" )]
	internal class StringSerializer : ISerializer<char>, ISerializer<string>, IDeserializer<string>
	{
		internal static readonly UTF8Encoding UTF8 = new();
		public Library ClassInfo { get; } = Library.Database[typeof( StringSerializer )];

		// Char

		public byte[] Serialize( char item )
		{
			throw new InvalidOperationException( "Why?" );
		}

		public byte[] Serialize( char[] item )
		{
			return UTF8.GetBytes( item );
		}

		// String

		public byte[] Serialize( string item )
		{
			return UTF8.GetBytes( item );
		}

		public string Deserialize( byte[] item )
		{
			return UTF8.GetString( item );
		}

		public byte[] Serialize( string[] item )
		{
			throw new NotImplementedException();
		}
	}
}
