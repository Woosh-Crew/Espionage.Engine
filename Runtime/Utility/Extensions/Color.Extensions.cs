using UnityEngine;

namespace Espionage.Engine
{
	public static class ColorExtensions
	{
		public static Color WithRed( this Color input, float value )
		{
			return new( value, input.g, input.b, input.a );
		}

		public static Color WithGreen( this Color input, float value )
		{
			return new( input.r, value, input.b, input.a );
		}

		public static Color WithBlue( this Color input, float value )
		{
			return new( input.r, input.g, value, input.a );
		}

		public static Color WithAlpha( this Color input, float value )
		{
			return new( input.r, input.g, input.b, value );
		}

		public static Color LerpTo( this Color input, Color b, float t )
		{
			return Color.Lerp( input, b, t );
		}

	}
}
