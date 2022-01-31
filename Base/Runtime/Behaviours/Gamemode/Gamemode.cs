using Espionage.Engine.Components;

namespace Espionage.Engine.Gamemodes
{
	public abstract class Gamemode : Behaviour, IComponent<World>
	{
		void IComponent<World>.OnAttached( World item ) { }
		
		// Validation
		
		public bool Validate()
		{
			return OnValidation();
		}

		protected virtual bool OnValidation()
		{
			return true;
		}
		
		// States

		public void Begin()
		{
			Debugging.Log.Info( $"Starting Gamemode - [{ClassInfo.Title}]" );
			OnBegin();	
		}
		
		protected virtual void OnBegin() { }

		public void Finish()
		{
			Debugging.Log.Info( $"Finishing Gamemode - [{ClassInfo.Title}]" );
			OnFinish();
		}
		
		protected virtual void OnFinish() { }
		
		//
		// Round
		//
	}
}
