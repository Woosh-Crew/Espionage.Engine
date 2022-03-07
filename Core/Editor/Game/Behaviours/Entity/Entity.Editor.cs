using System;
using Espionage.Engine.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEditor;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Entity ), true )]
	public class EntityEditor : UnityEditor.Editor
	{
		private Library ClassInfo { get; set; }

		private void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] = $"{ClassInfo.Title} (Entity)";
		}

		public override void OnInspectorGUI()
		{

			base.OnInspectorGUI();
		}
	}
}
