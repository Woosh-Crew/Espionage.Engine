using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

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
					return _titles;

				_titles = TitlesCache();

				return _titles;
			}
		}

		private static Dictionary<Type, string> TitlesCache()
		{
			var inspectorTitlesType = typeof( ObjectNames ).GetNestedType( "InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
			var inspectorTitlesField = inspectorTitlesType.GetField( "s_InspectorTitles", BindingFlags.Static | BindingFlags.NonPublic );
			return (Dictionary<Type, string>)inspectorTitlesField.GetValue( null );
		}
	}
}
