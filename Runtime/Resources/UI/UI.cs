using System;

namespace Espionage.Engine.Resources
{
	[Group( "User Interfaces" ), Title( "User Interface" ), Path( "ui", "assets://Interfaces/" )]
	public partial class UI : Resource
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
				Debugging.Log.Error( $"UI Path [{Files.Pathing.Absolute( path )}], couldn't be found." );
				return null;
			}

			var file = Files.Serialization.Load<File>( path );
			return new( file.Binder );
		}

		//
		// Instance
		//

		public override string Identifier { get; }
		private Binder Provider { get; }

		private UI( Binder provider )
		{
			Provider = provider ?? throw new NullReferenceException();
			Database.Add( this );
		}

		public override void Load( Action loaded = null )
		{
			base.Load( loaded );
			Provider.Load( loaded );
		}

		public override void Unload( Action unloaded = null )
		{
			base.Unload( unloaded );
			Provider.Unload( unloaded );
		}
	}
}
