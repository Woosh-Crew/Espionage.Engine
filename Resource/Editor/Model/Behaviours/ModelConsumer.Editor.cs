using Espionage.Engine.Editor;
using Espionage.Engine.Resources;
using UnityEditor;
using UnityEngine;

namespace Espionage.Engine.Resources.Editor
{
	[CustomEditor( typeof( ModelConsumer ), true )]
	public class ModelConsumerEditor : ComponentEditor
	{
		protected ModelConsumer Consumer => target as ModelConsumer;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if ( GUILayout.Button( "Load Model" ) )
			{
				Consumer.Model = Model.Load( Consumer.Path );
			}
		}
	}
}
