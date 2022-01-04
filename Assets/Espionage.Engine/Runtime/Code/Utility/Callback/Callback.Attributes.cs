// Attribute based event callback system

using System;

namespace Espionage.Engine
{
	public static partial class Callback
	{
		public class Frame : CallbackAttribute
		{
			public const string Identifier = "event.frame";
			public Frame() : base( Identifier ) { }
		}
	}


	[AttributeUsage( AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
	public class CallbackAttribute : Attribute
	{
		readonly string name;
		public string Name => name;

		public CallbackAttribute( string name )
		{
			this.name = name;
		}
	}
}
