using System.Text;
using Espionage.Engine.Logging;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Terminal : Tool
	{
		private string _input = string.Empty;
		private bool _scrollToBottom;

		public override void OnLayout()
		{
			// Log Output

			ImGui.BeginChild( "Output", new( 0, 512 ), true, ImGuiWindowFlags.ChildWindow );
			{
				foreach ( var entry in Dev.Log.All )
				{
					ImGui.TextColored( Entry.Colors[entry.Type], entry.Message );
					if ( ImGui.IsItemHovered() )
					{
						ImGui.SetTooltip( entry.StackTrace );
					}
				}

				if ( _scrollToBottom && ImGui.GetScrollY() >= ImGui.GetScrollMaxY() )
				{
					ImGui.SetScrollHereY( 1.0f );
				}

			}
			ImGui.EndChild();

			ImGui.Separator();

			// Command Line

			var reclaimFocus = false;

			if ( ImGui.InputTextWithHint( string.Empty, "Enter Command...", ref _input, 160, ImGuiInputTextFlags.EnterReturnsTrue ) )
			{
				Dev.Log.Info( _input, "Inputted Text" );
				Dev.Terminal.Invoke( _input );
				_input = string.Empty;
				_scrollToBottom = true;
				reclaimFocus = true;
			}

			ImGui.SetItemDefaultFocus();
			if ( reclaimFocus )
			{
				ImGui.SetKeyboardFocusHere( -1 );
			}

			// Hinting

			if ( string.IsNullOrEmpty( _input ) )
			{
				return;
			}

			var lastItem = ImGui.GetItemRectMin();
			ImGui.SetNextWindowPos( lastItem + new Vector2( 0, ImGui.GetItemRectSize().y ) );

			// Lotta Flags.. Kinda looks like a flag
			const ImGuiWindowFlags flags = ImGuiWindowFlags.NoBackground |
			                               ImGuiWindowFlags.Tooltip |
			                               ImGuiWindowFlags.NoDecoration |
			                               ImGuiWindowFlags.AlwaysAutoResize |
			                               ImGuiWindowFlags.NoSavedSettings |
			                               ImGuiWindowFlags.NoFocusOnAppearing |
			                               ImGuiWindowFlags.NoNav;

			if ( ImGui.Begin( string.Empty, flags ) )
			{
				foreach ( var item in Dev.Terminal.Find( _input ) )
				{
					var stringBuilder = new StringBuilder( $"{item} ( " );
					var command = Dev.Terminal.Get( item );

					foreach ( var parameter in command.Parameters )
					{
						stringBuilder.Append( $"{parameter} " );
					}

					ImGui.Text( $"{stringBuilder})" );
				}

				ImGui.End();
			}
		}
	}
}
