using System;
using Espionage.Engine.Components;
using Espionage.Engine.Internal.Commands;

namespace Espionage.Engine
{
	/// <summary>
	/// A Var is basically a <see cref="ConCmd.ConCmdAttribute"/>, with the command prebuilt when initializing.
	/// This var allows you to change and read a property at any time. You can also serialize the value
	/// for persistence. Attribute must be attached to a static property.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public sealed class ConVarAttribute : Attribute, IComponent<Property>
	{
		public void OnAttached( Property item )
		{
			if ( !item.IsStatic )
			{
				Debugging.Log.Error( $"Property \"{item.Name}\" Must be Static!" );
				return;
			}

			var command = new Command()
			{
				Name = item.Name,
				Help = item.Help,
				Info = item.Info
			}.WithAction(
				( parameters ) =>
				{
					if ( parameters is not null && parameters.Length > 0 )
					{
						var value = parameters[0];
						item[null] = value;

						Debugging.Log.Info( $"{item.Name} is now {value}" );
					}
					else
					{
						Debugging.Log.Info( $"{item.Name} is now {item[null]}" );
					}
				}
			);

			Debugging.Console.Add( command );
		}
	}
}
