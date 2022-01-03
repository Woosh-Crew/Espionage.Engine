// Attribute based event callback system

using System;
using System.Collections.Generic;
using System.Reflection;
using Espionage.Engine.Internal.Callbacks;

namespace Espionage.Engine
{
	public static partial class Callback
	{
		public class FrameAttribute : CallbackAttribute
		{
			public FrameAttribute() : base( "base.update" ) { }
		}
	}


	[AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
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
