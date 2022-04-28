namespace Espionage.Engine
{
	public readonly struct Output
	{
		public Output( string name, string target, string function, float delay )
		{
			Name = name;

			Target = target;
			Input = function;
			Delay = delay;
		}

		public string Name { get; }
		private string Target { get; }
		private string Input { get; }
		private float Delay { get; }

		// API

		public void Invoke()
		{
			foreach ( var entity in Entity.All[Target] )
			{
				var func = entity.ClassInfo.Functions[Input];

				if ( func == null )
				{
					Debugging.Log.Warning( $"Input [{Input}] couldn't be invoked. No function with matching name was found" );
					continue;
				}

				entity.ClassInfo.Functions[Input]?.Invoke( entity );
			}
		}
	}
}
