using System;
using Espionage.Engine.Internal;
using NUnit.Framework.Internal.Filters;
using UnityEditor;

namespace Espionage.Engine.Editor
{
	[CustomEditor( typeof( Component ), true )]
	public class ComponentEditor : UnityEditor.Editor
	{
		private Library ClassInfo { get; set; }
		
		private void OnEnable()
		{
			ClassInfo = Library.Database[target.GetType()];
			EditorInjection.Titles[target.GetType()] =$"{ClassInfo.Title} - [ Component<Viewmodel> ]";
		}

		public override void OnInspectorGUI()
		{
			
			base.OnInspectorGUI();
		}
	}
}
