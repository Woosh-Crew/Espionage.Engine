using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Internal
{
	public static class EditorInjection
	{
		// Titles Cache

		private static Dictionary<Type, string> _titles;

		public static Dictionary<Type, string> Titles
		{
			get
			{
				if ( _titles != null )
				{
					return _titles;
				}

				_titles = TitlesCache();

				return _titles;
			}
		}

		private static Dictionary<Type, string> TitlesCache()
		{
			var type = typeof( ObjectNames ).GetNestedType( "InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
			var titles = type.GetField( "s_InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
			return (Dictionary<Type, string>)titles?.GetValue( null );
		}
	}
}
