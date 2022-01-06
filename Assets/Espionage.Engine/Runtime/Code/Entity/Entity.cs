using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Espionage.Engine.Internal;

using Object = UnityEngine.Object;

namespace Espionage.Engine
{
	public abstract class Entity : Object, ILibrary, ICallbacks
	{
		public string Name { get; }
		public HashSet<string> Tags { get; }

		public static IReadOnlyList<Entity> All => _all;
		private static List<Entity> _all = new List<Entity>();

		//
		// Spawn
		//

		/// <summary> This is called on the Server just before the Server spawns the object. 
		/// Use this to initialize values. Or run any initialization operations. </summary>
		public virtual void Spawn() { }

		//
		// Simulate
		//

		/// <summary> This is called both on the owning client and server every tick. </summary>
		/// <param name="cl"> The current processsing client </param>
		public virtual void Simulate( Client cl ) { }

		// 
		// Management
		//

		private EntityLink _link;
		internal void SetLink( EntityLink link ) => _link = link;

		public Entity()
		{
			_all.Add( this );

			EntityLink.Create( this );
		}

		private void OnDestory()
		{
			_all.Remove( this );
		}

		//
		// Helpers
		//

		public Library ClassInfo => Library.Accessor.Get( GetType() );

		public Pawn Owner { get; set; }
		public Transform Transform => _link.transform;
	}
}
