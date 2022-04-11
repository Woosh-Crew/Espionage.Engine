namespace Espionage.Engine
{
	/// <summary>Fire type for a given firearm</summary>
	public enum FireType{
		//Raycasts
		Bullet = 0,
		//Objects, physical projectiles
		Projectile = 1,
		///Anything requiring the fire button to be held like a laser beam, flame thrower, or grav gun type ordeal
		Stream = 2
	}

	public class BaseFirearm : Weapon
	{
		

	}
}
