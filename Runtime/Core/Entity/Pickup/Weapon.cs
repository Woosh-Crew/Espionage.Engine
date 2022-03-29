namespace Espionage.Engine
{
	public abstract class Weapon : Holdable
	{
		/// <summary>The damage a given weapon does</summary>
		public virtual float Damage{get; set;} = 1f;

		/// <summary>Time since our primary attack</summary>
		private TimeSince TimeSincePrimaryAttack;

		/// <summary>Time since our secondary attack</summary>
		private TimeSince TimeSinceSecondaryAttack;

		/// <summary>The primary attack for this weapon</summary>
		public virtual void PrimaryAttack(){

		}

		/// <summary>The secondary attack for this weapon</summary>
		public virtual void SecondaryAttack(){

		}
		
		public virtual bool CanPrimaryAttack(){
			return true;
		}

		public virtual bool CanSecondaryAttack(){
			return true;
		}

	}
}
