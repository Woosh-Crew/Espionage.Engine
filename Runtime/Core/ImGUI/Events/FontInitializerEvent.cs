using ImGuiNET;
using UnityEngine.Events;


namespace Espionage.Engine.ImGUI.Events
{
	[System.Serializable]
	public class FontInitializerEvent : UnityEvent<ImGuiIOPtr> { }
}