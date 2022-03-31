using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public class Toolbar : Window
	{
		public Toolbar()
		{
			_tools = Library.Database.GetAll<Window>().Where( e => e.Components.Has<IconAttribute>() ).ToList();
		}

		private readonly List<Library> _tools;

		public override void OnLayout()
		{
			foreach ( var tool in _tools )
			{
				ImGui.SameLine();
				ImGui.Button( tool.Title );
			}
		}
	}
}
