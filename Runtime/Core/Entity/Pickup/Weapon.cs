namespace Espionage.Engine
{
	public abstract class Weapon : Holdable
	{
		/// <summary>The damage a given weapon does</summary>
		public virtual float Damage => 1f;

		/// <summary>The primary attack for this weapon</summary>
		public virtual void PrimaryAttack(){

		}

		/// <summary>The secondary attack for this weapon</summary>
		public virtual void SecondaryAttack(){

		}

	}
}
