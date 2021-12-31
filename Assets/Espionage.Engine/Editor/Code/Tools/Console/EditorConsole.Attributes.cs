using System;
using System.Reflection;
using Espionage.Engine;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	// These attributes only compile for the Editor

	public static partial class EditorConsole
	{
		/// <summary> Editor only command </summary>
		[AttributeUsage( AttributeTargets.Method, Inherited = false, AllowMultiple = false )]
		public class CmdAttribute : Console.CmdAttribute
		{
			public CmdAttribute( params string[] names ) : base( names ) { }
		}


		/// <summary> Editor only variable </summary>
		[AttributeUsage( AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
		public sealed class VarAttribute : Console.VarAttribute
		{
			public VarAttribute( string name ) : base( name ) { }
		}
	}
}
