using System;
using UnityEngine;

namespace Espionage.Engine.World
{
	/// <summary>
	/// Environment generates world lighting. This includes
	/// the Directional light, skybox, fog, Post Processing, etc.
	/// </summary>
	[Group( "Maps.Lighting" ), CreateAssetMenu]
	public class Environment : ScriptableObject, ILibrary
	{
		public Library ClassInfo { get; private set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database[GetType()];
		}
		
		// Lighting
		
		[SerializeField]
		private Material skybox;
	}
}
