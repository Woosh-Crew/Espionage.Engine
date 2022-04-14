using System;

namespace Espionage.Engine.Gamemodes
{
	public class TimedRound : Round
	{
		public virtual TimeSpan Duration { get; set; }
		public override TimeSpan Time => !Finished ? TimeSpan.FromSeconds( Duration.TotalSeconds - base.Time.TotalSeconds ) : TimeSpan.Zero;

		// Logic
		
		private float _endTime;
		
		protected override void OnStart()
		{
			if ( Duration.TotalSeconds > 0 )
				_endTime = UnityEngine.Time.time + (float)Duration.TotalSeconds;
		}

		protected override void OnFinish()
		{
			_endTime = 0f;
		}

		protected sealed override void OnThink()
		{
			if ( _endTime > 0 && UnityEngine.Time.time >= _endTime )
			{
				_endTime = 0f;
				OnTimeUp();
			}
		}

		protected virtual void OnTimeUp() { Finish(); }
	}
}
