using System;
using System.Linq;

namespace Espionage.Engine.Converters
{
	public sealed class SystemConverter : IConverter<bool>, IConverter<int>, IConverter<float>, IConverter<double>, IConverter<long>, IConverter<string>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( SystemConverter )];

		string IConverter<string>.Convert( string value ) { return value; }

		//
		// Bool
		//

		public readonly string[] Pass = new[]
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

		public readonly string[] Block = new[]
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
			if ( value.Length == 1 )
			{
				return int.Parse( value ) == 1;
			}

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

			throw new InvalidCastException();
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
