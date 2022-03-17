using UnityEngine;

namespace Espionage.Engine.Resources.Formats
{
	[Title( "Canvas" ), CreateAssetMenu( menuName = "Espionage.Engine/Assets/Canvas Document" )]
	public sealed class CanvasAsset : ScriptableObject
	{
		public Canvas UI => ui;

		// Fields

		[SerializeField]
		private Canvas ui;
	}
}
