using UnityEngine;

namespace Espionage.Engine.ImGUI.Assets
{
	[CreateAssetMenu(menuName = "Dear ImGui/Shader Resources")]
	internal sealed class ShaderResourcesAsset : ScriptableObject
	{
		public ShaderData Shader;
		public ShaderProperties PropertyNames;
	}
}
