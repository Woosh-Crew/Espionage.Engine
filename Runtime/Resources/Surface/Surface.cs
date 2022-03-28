using UnityEngine;

namespace Espionage.Engine
{
	[CreateAssetMenu( menuName = "Espionage.Engine/Surface" )]
	public class Surface : ScriptableObject
	{
		public float Friction => friction;
		public float Density => density;
		public AudioClip[] Footsteps => footstepSounds;
		public AudioClip[] ImpactSounds => impactSounds;
		public ParticleSystem[] ImpactEffects => impactEffects;

		// Fields

		[SerializeField]
		private float friction = 1f;

		[SerializeField]
		private float density = 100f;

		[SerializeField]
		private AudioClip[] footstepSounds;

		[SerializeField]
		private AudioClip[] impactSounds;

		[SerializeField]
		private ParticleSystem[] impactEffects;

		public static implicit operator PhysicMaterial( Surface surface )
		{
			return new( surface.name ) { staticFriction = surface.friction };
		}
	}

	public interface ISurface
	{
		Surface Surface { get; }
	}
}
