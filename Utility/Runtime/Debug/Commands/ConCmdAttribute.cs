using System;
using Espionage.Engine.Components;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Method, Inherited = false )]
	public class ConCmdAttribute : Attribute, IComponent<Function>
	{
		public void OnAttached( Function item )
		{
			Debugging.Log.Info( "Adding Command" );

			if ( !item.Info.IsStatic )
			{
				Debugging.Log.Error( $"Function \"{item.Name}\" Must be Static!" );
				return;
			}

			var command = new Command()
			{
				Name = item.Name,
				Help = item.Help,
				Info = item.Info
			}.WithAction(
				( e ) => item.Info?.Invoke( null, e )
			);

			Debugging.Console.Add( command );
		}
	}
}
