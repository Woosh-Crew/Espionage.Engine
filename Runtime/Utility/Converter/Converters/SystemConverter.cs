using System;
using System.Linq;

namespace Espionage.Engine.Converters
{
	[Library( "converter.system" ), Group( "Converters" ), Singleton]
	internal sealed class SystemConverter : IConverter<bool>, IConverter<int>, IConverter<float>, IConverter<double>, IConverter<long>, IConverter<string>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( SystemConverter )];

		string IConverter<string>.Convert( string value ) { return value; }

		//
		// Bool
		//

		private static readonly string[] Pass =
		{
			"true",
			"accept",
			"grant",
			"correct",
			"positive",
			"1",
			"yes",
			"y"
		};

		private static readonly string[] Block =
		{
			"false",
			"wrong",
			"negative",
			"0",
			"no",
			"n"
		};

		bool IConverter<bool>.Convert( string value )
		{
			value = value.ToLower();

			// If Pass Matches
			if ( Pass.Any( e => e == value ) )
			{
				return true;
			}

			// If any Block Matches
			if ( Block.Any( e => e == value ) )
			{
				return false;
			}

			throw new InvalidCastException( $"No Valid casting for input \"{value}\"" );
		}

		//
		// Value
		//

		int IConverter<int>.Convert( string value ) { return int.Parse( value ); }
		float IConverter<float>.Convert( string value ) { return float.Parse( value ); }
		double IConverter<double>.Convert( string value ) { return double.Parse( value ); }
		long IConverter<long>.Convert( string value ) { return long.Parse( value ); }
	}
}
