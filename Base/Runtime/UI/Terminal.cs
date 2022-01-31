using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Espionage.Engine.UI
{
	[Library]
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
				show = !show;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Callback.Unregister( this );
		}

		private bool show;
		private string field;

		private void OnGUI()
		{
			if ( !show )
			{
				return;
			}

			// This is the worst
			field = GUILayout.TextField( field, GUILayout.ExpandWidth( true ) );
		}

		//
		// Init
		//

		[Callback( "engine.layer_created" )]
		public static void Initialize()
		{
			Debugging.Log.Info( "Creating Terminal" );

			var terminal = new GameObject( "Terminal" ).AddComponent<Terminal>();
			Engine.AddToLayer( terminal.gameObject );
		}
	}
}
