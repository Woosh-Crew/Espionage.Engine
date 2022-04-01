﻿using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public sealed class Model : Resource<GameObject>
	{
		public static Model Load( string path, Action<GameObject> onLoad = null )
		{
			return null;
		}

		public int Instances { get; set; }
		public GameObject Object { get; set; }

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