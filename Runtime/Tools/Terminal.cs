using System;
using System.Linq;
using System.Text;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Terminal : Window
	{
		public bool Focus { get; set; }

		private string _input = string.Empty;
		private string _search = string.Empty;
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

			ImGui.SetNextItemWidth( ImGui.GetWindowWidth() - 48 * 2 - 28 );

			ImGui.SetItemDefaultFocus();
			ImGui.InputTextWithHint( "Search", "Search Output...", ref _search, 160 );

			if ( ImGui.BeginChild( "out", new( 0, ImGui.GetWindowHeight() - 96 ), false ) )
			{
				if ( ImGui.BeginTable( "Output", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable ) )
				{
					ImGui.TableSetupColumn( "Time", ImGuiTableColumnFlags.WidthFixed, 72 );
					ImGui.TableSetupColumn( "Type", ImGuiTableColumnFlags.WidthFixed, 96 );
					ImGui.TableSetupColumn( "Message" );

					ImGui.TableHeadersRow();

					foreach ( var entry in string.IsNullOrEmpty( _search )
						         ? Dev.Log.All
						         : Dev.Log.All.Where( e => e.Message.Contains( _search, StringComparison.CurrentCultureIgnoreCase ) || e.Level.StartsWith( _search, StringComparison.CurrentCultureIgnoreCase ) ) )
					{
						ImGui.TableNextColumn();
						ImGui.TextColored( Color.gray, $"[{DateTime.Now.ToShortTimeString()}]" );

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
					var stringBuilder = new StringBuilder( $"{item}" );
					var command = Dev.Terminal.Get( item );

					if ( command.Parameters.Length > 0 )
					{
						stringBuilder.Append( " [ " );

						foreach ( var parameter in command.Parameters )
						{
							stringBuilder.Append( $"{parameter.Name} " );
						}

						stringBuilder.Append( "]" );
					}

					ImGui.Text( stringBuilder.ToString() );
				}

				ImGui.End();
			}
		}
	}
}
