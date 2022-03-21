using System;
using Espionage.Engine.ImGUI.Texture;

namespace Espionage.Engine.ImGUI
{
	internal sealed class Context
	{
		public IntPtr ImGuiContext;
		public IntPtr ImNodesContext;
		public IntPtr ImPlotContext;
		public TextureManager TextureManager;
	}
}