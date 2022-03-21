using System;

namespace Espionage.Engine.ImGUI
{
	[Serializable]
	internal class ShaderProperties
	{
		public string Texture;
		public string Vertices;
		public string BaseVertex;

		public ShaderProperties Clone()
		{
			return (ShaderProperties)MemberwiseClone();
		}
	}
}
