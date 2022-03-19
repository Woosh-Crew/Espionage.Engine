namespace Espionage.Engine
{
	public interface IHealable
	{
		void Heal( Info amount );

		public struct Info
		{
			public static implicit operator Info( int value )
			{
				return new()
				{
					Amount = value
				};
			}

			public int Amount { get; set; }
			public Actor Healer { get; set; }
		}
	}
}
