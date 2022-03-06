﻿using System;
using System.Reflection;

namespace Espionage.Engine
{
	/// <summary>
	/// Properties are variables that are changeable by the editor.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public sealed class PropertyAttribute : Attribute
	{
		public string Name { get; }
		public PropertyAttribute() { }

		public PropertyAttribute( string name )
		{
			Name = name;
		}

		public Property CreateRecord( Library library, PropertyInfo info )
		{
			return new( library, info, !string.IsNullOrEmpty( Name ) ? Name : info.Name );
		}
	}
}