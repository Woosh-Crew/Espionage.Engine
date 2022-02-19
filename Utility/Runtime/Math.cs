namespace Espionage.Engine
{
	public static class Math
	{
		public static float Remap( float input, float inputMin, float inputMax, float min, float max )
		{
			return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
		}
	}
}
