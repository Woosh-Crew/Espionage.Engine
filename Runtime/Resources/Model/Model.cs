using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public sealed class Model : Resource<GameObject>
	{
		public static Model Load( string path, Action<GameObject> onLoad )
		{
			return null;
		}

		private int Instances { get; set; }
		private GameObject Object { get; set; }

		protected override void Load( Action<GameObject> onLoad )
		{
			if ( Instances <= 0 )
			{
				// Add to Database
				Database.Add( this );
			}

			Instances++;
		}

		protected override void Unload()
		{
			Instances--;

			if ( Instances <= 0 )
			{
				// Remove from Database
				Database.Remove( this );
			}
		}
	}
}
