using System;

namespace Espionage.Engine.Gamemodes
{
	public abstract class Round : Component<Gamemode>
	{
		public virtual TimeSpan Time => !Finished ? TimeSpan.FromSeconds( Started ) : TimeSpan.Zero;
		
		public TimeSince Started { get; private set; }
		public bool Finished { get; private set; }

		public void Start()
		{
			Debugging.Log.Info( $"Round started {GetType()}" );

			Started = 0;
			OnStart();
			
			Entity.Thinking.Add( OnThink, 1 );
		}

		public virtual void Finish()
		{
			if ( Finished )
			{
				return;
			}

			Debugging.Log.Info( $"Round ended {GetType()}" );

			Finished = true;
			OnFinish();
		}

		protected virtual void OnThink() { }
		protected virtual void OnStart() { }
		protected virtual void OnFinish() { }
	}
}
