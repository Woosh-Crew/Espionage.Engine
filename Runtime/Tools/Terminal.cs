using System.Text;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Terminal : Window
	{
		public bool Focus { get; set; }

		private string _input = string.Empty;
		private bool _scrollToBottom;

		private void Send()
		{
			Dev.Log.Add( new()
			{
				Message = $"> {_input}",
				StackTrace = "Inputted Text",
				Level = "Input",
				Color = Color.cyan
			} );

			Dev.Terminal.Invoke( _input );

			_input = string.Empty;
			_scrollToBottom = true;
			Focus = true;
		}

		public override void OnLayout()
		{
			// Log Output

			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 72 ), false ) )
			{
				if ( ImGui.BeginTable( "Output", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetBgColor( ImGuiTableBgTarget.RowBg0, 0 );
					ImGui.TableSetupColumn( "Type", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Message" );

					ImGui.TableHeadersRow();

					foreach ( var entry in Dev.Log.All )
					{
						// Log Type
						ImGui.TableNextColumn();
						ImGui.TextColored( entry.Color == default ? Color.white : entry.Color, entry.Level ?? "None" );

						// Message
						ImGui.TableNextColumn();
						ImGui.TextWrapped( entry.Message ?? "None" );

						if ( ImGui.IsItemHovered() && !string.IsNullOrEmpty( entry.StackTrace ) )
						{
							ImGui.SetTooltip( entry.StackTrace );
						}

						ImGui.TableNextRow();
					}
				}

				ImGui.EndTable();

				if ( _scrollToBottom && ImGui.GetScrollY() >= ImGui.GetScrollMaxY() )
				{
					ImGui.SetScrollHereY( 1.0f );
				}
			}

			ImGui.EndChild();

			ImGui.Separator();

			// Command Line

			ImGui.BeginGroup();
			{
				ImGui.SetNextItemWidth( ImGui.GetWindowWidth() - 48 * 2 - 28 );

				ImGui.SetItemDefaultFocus();
				if ( ImGui.InputTextWithHint( string.Empty, "Enter Command...", ref _input, 160, ImGuiInputTextFlags.EnterReturnsTrue ) )
				{
					Send();
				}

				if ( Focus )
				{
					Focus = false;
					ImGui.SetKeyboardFocusHere( -1 );
				}

				ImGui.SameLine();

				ImGui.SetNextItemWidth( 48 );
				if ( ImGui.Button( "Submit" ) )
				{
					Send();
				}

				ImGui.SameLine();

				ImGui.SetNextItemWidth( 48 );
				if ( ImGui.Button( "Clear" ) )
				{
					Dev.Terminal.Invoke( "clear" );
				}
			}
			ImGui.EndGroup();

			// Hinting

			if ( string.IsNullOrEmpty( _input ) )
			{
				return;
			}

			var lastItem = ImGui.GetItemRectMin();

			ImGui.SetNextWindowPos( lastItem + new Vector2( 0, ImGui.GetItemRectSize().y + 8 ) );

			// Lotta Flags.. Kinda looks like a flag
			const ImGuiWindowFlags flags =
				ImGuiWindowFlags.Tooltip |
				ImGuiWindowFlags.NoDecoration |
				ImGuiWindowFlags.AlwaysAutoResize |
				ImGuiWindowFlags.NoSavedSettings |
				ImGuiWindowFlags.NoFocusOnAppearing |
				ImGuiWindowFlags.NoNav;

			ImGui.SetNextWindowBgAlpha( 0.5f );

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
