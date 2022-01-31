using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.UI
{
	[RequireComponent(typeof(UIDocument))]
	public class Terminal : SingletonComponent<Terminal>, ILibrary, ICallbacks
	{
		public Library ClassInfo { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			ClassInfo = Library.Database[GetType()];
			Callback.Register( this );
		}

		private void Update()
		{
			if ( Input.GetKeyDown( KeyCode.F1 ) )
			{
				Debugging.Log.Info( "Open Console" );
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			Callback.Unregister( this );
		}
	}
}
