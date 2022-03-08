using System;
using UnityEngine;

namespace Espionage.Engine.Converters
{
	[Library( "converter.unity" ), Group( "Converters" )]
	internal sealed class UnityConverter : IConverter<Vector2>, IConverter<Vector3>, IConverter<Vector4>, IConverter<Quaternion>, IConverter<Color>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( UnityConverter )];

		//
		// Quaternion
		//

		Quaternion IConverter<Quaternion>.Convert( string value )
		{
			value = value[1..^1];

			var split = value.Split( "," );

			if ( split.Length < 3 )
			{
				throw new InvalidCastException();
			}

			return Quaternion.Euler(
				float.Parse( split[0] ),
				float.Parse( split[1] ),
				float.Parse( split[2] )
			);
		}

		//
		// Vector 3
		//

		Vector2 IConverter<Vector2>.Convert( string value )
		{
			var split = value.Split( "," );

			if ( split.Length < 2 )
			{
				throw new InvalidCastException();
			}

			return new Vector2(
				float.Parse( split[0] ),
				float.Parse( split[1] )
			);
		}

		Vector3 IConverter<Vector3>.Convert( string value )
		{
			value = value[1..^1];

			var split = value.Split( "," );

			if ( split.Length < 3 )
			{
				throw new InvalidCastException();
			}

			return new Vector3(
				float.Parse( split[0] ),
				float.Parse( split[1] ),
				float.Parse( split[2] )
			);
		}

		Vector4 IConverter<Vector4>.Convert( string value )
		{
			var split = value.Split( "," );

			if ( split.Length < 4 )
			{
				throw new InvalidCastException();
			}

			return new Vector4(
				float.Parse( split[0] ),
				float.Parse( split[1] ),
				float.Parse( split[2] ),
				float.Parse( split[3] )
			);
		}

		//
		// Color
		//

		Color IConverter<Color>.Convert( string value )
		{
			var split = value.Split( "," );

			if ( split.Length < 3 )
			{
				throw new InvalidCastException();
			}

			return new Color(
				float.Parse( split[0] ),
				float.Parse( split[1] ),
				float.Parse( split[2] )
			);
		}
	}
}
