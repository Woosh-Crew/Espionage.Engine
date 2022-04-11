using System;
using Espionage.Engine.Components;
using Espionage.Engine.IO;

namespace Espionage.Engine
{
	public class KeywordAttribute : Attribute, IComponent<Function>
	{
		public string Key { get; }

		public KeywordAttribute( string key )
		{
			Key = key;
		}

		public void OnAttached( Function item )
		{
			if ( item.Info.ReturnType != typeof( string ) )
			{
				Debugging.Log.Error( $"Keyword [{Key}] on Function {item.Name} doesn't have a string return type!" );
				return;
			}

			if ( item.Info.GetParameters().Length == 0 )
			{
				Files.Pathing.Add( Key, _ => item.Invoke<string>( null ) );
				return;
			}

			Files.Pathing.Add( Key, args => item.Invoke<string>( null, args ) );
		}
	}
}
