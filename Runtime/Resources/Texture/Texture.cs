using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Resources
{
	[Group( "Textures" ), Title( "User Interface" ), Path( "textures", "assets://Textures/" )]
	public partial class Texture : Resource<Texture2D>
	{
		/// <summary>
		/// Trys to find the UI by path. If it couldn't find the UI in the database,
		/// it'll create a new reference to that UI for use later.
		/// </summary>
		public static Texture Find( string path )
		{
			path = Files.Pathing.Absolute( path );

			// Use the Database Map if we have it
			if ( Database[path] != null )
			{
				return Database[path] as Texture;
			}

			if ( !Files.Pathing.Exists( path ) )
			{
				Dev.Log.Error( $"Texture Path [{Files.Pathing.Absolute( path )}], couldn't be found." );
				return null;
			}

			var file = Files.Serialization.Load<File>( path );
			return new( file.Binder );
		}

		//
		// Instance
		//

		public override string Identifier => Provider.Identifier;
		public Binder Provider { get; }

		private Texture( Binder provider )
		{
			Provider = provider ?? throw new NullReferenceException();
		}

		public override void Load( Action<Texture2D> loaded = null )
		{
			if ( IsLoaded )
			{
				loaded?.Invoke( Provider.Texture );
				return;
			}

			using var _ = Dev.Stopwatch( $"Loaded Texture [{Identifier}]" );
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
