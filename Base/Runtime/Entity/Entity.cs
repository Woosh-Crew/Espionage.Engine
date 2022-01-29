using System.Collections;
using System.Collections.Generic;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	public abstract class Entity : Object, ILibrary, ICallbacks
	{
		public static List<Entity> All { get; }

		static Entity()
		{
			All = new List<Entity>();
		}

		// Entity

		public Library ClassInfo { get; set; }

		public Entity()
		{
			ClassInfo = Library.Database[GetType()];
			Components = new ComponentDatabase<Entity>( this );

			Callback.Register( this );
			All.Add( this );

			CreateHook();
		}

		private void OnDestroy()
		{
			// Clear Components, just in case.
			Components.Clear();

			All.Remove( this );
			Callback.Unregister( this );

			DeleteHook();
		}

		// Transform

		public Transform Transform => _gameObject.transform;

		public Vector3 Position => Transform.position;
		public Quaternion Rotation => Transform.rotation;
		public Vector3 Scale => Transform.localScale;

		// Hook

		public GameObject Hook => _gameObject;
		private GameObject _gameObject;

		private void CreateHook()
		{
			_gameObject = new GameObject( ClassInfo.Name );
			var reference = _gameObject.AddComponent<EntityReference>();
			reference.Entity = this;

			// Add Entity reference component
		}

		private void DeleteHook()
		{
			Destroy( _gameObject );
		}

		// Validation

		public virtual bool IsValid()
		{
			return _gameObject != null;
		}

		//
		// Components
		//

		public ComponentDatabase<Entity> Components { get; }
	}
}
