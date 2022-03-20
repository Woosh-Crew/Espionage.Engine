namespace Espionage.Engine
{
	/// <summary>
	/// E Math, meaning Espionage.Engine Math.
	/// </summary>
	public class EMath
	{
		public static float Remap( float input, float inputMin, float inputMax, float min, float max )
		{
			return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
		}
	}
}
