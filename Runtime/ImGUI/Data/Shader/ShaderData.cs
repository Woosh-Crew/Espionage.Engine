using System;
using UnityEngine;

namespace Espionage.Engine.ImGUI
{
	[Serializable]
	internal class ShaderData
	{
		public Shader Mesh;
		public Shader Procedural;

		public ShaderData Clone()
		{
			return (ShaderData)MemberwiseClone();
		}
	}
}
