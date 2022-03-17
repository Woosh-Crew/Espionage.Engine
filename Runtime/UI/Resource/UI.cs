﻿using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "User Interface" ), Path( "ui", "assets://User Interfaces/" )]
	public partial class UI : Resource<GameObject>
	{
		/// <summary>
		/// Trys to find the UI by path. If it couldn't find the UI in the database,
		/// it'll create a new reference to that UI for use later.
		/// </summary>
		public static UI Find( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				return Database[path] as UI;
			}

			if ( !Files.Pathing.Exists( path ) )
			{
				Dev.Log.Error( $"UI Path [{Files.Pathing.Absolute( path )}], couldn't be found." );
				return null;
			}

			var file = Files.Serialization.Load<File>( path );
			return new( file.Binder );
		}

		//
		// Instance
		//

		public override string Identifier => Provider.Identifier;
		private Binder Provider { get; }

		private UI( Binder provider )
		{
			Provider = provider ?? throw new NullReferenceException();
		}

		public override void Load( Action<GameObject> loaded = null )
		{
			if ( IsLoaded )
			{
				loaded?.Invoke( Provider.Canvas );
				return;
			}

			using var _ = Dev.Stopwatch( $"Loaded UI [{Identifier}]" );
			Provider.Load( loaded );
			base.Load( loaded );
		}

		public override void Unload( Action unloaded = null )
		{
			base.Unload( unloaded );
			Provider.Unload( unloaded );
		}
	}
}