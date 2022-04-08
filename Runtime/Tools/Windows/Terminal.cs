using System;
using System.Linq;
using System.Reflection;
using System.Text;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class Terminal : Window
	{
		public bool Focus { get; set; }
		public override ImGuiWindowFlags Flags => base.Flags | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavInputs;

		private string _input = string.Empty;
		private string _search = string.Empty;
		private bool _scrollToBottom;

		private void Send()
		{
			// Send Input to Output
			Dev.Log.Add( new()
			{
				Message = $"> {_input}",
				Trace = "Inputted Text",
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
			ImGui.SetNextItemWidth( ImGui.GetWindowWidth() - 16 );

			ImGui.InputTextWithHint( "Search", "Log Search...", ref _search, 160 );

			// Us doing this removes the title.. but we gotta or else the scrolling just doesnt work
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

						if ( ImGui.IsItemHovered() && !string.IsNullOrEmpty( entry.Trace ) )
						{
							ImGui.SetTooltip( entry.Trace );
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
				ImGui.PushStyleVar( ImGuiStyleVar.FramePadding, new Vector2( 8, 4 ) );

				if ( ImGui.InputTextWithHint( string.Empty, "Enter Command...", ref _input, 160, ImGuiInputTextFlags.EnterReturnsTrue ) )
				{
					Send();
				}

				ImGui.PopStyleVar();
				ImGui.SetItemDefaultFocus();

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

			ImGui.SetNextWindowPos( lastItem + new Vector2( 0, ImGui.GetItemRectSize().y - 22 - 8 ), ImGuiCond.Always, Vector2.up );
			ImGui.SetNextWindowSize( new( ImGui.GetWindowWidth() - 16, 0 ) );

			// Lotta Flags.. Kinda looks like a flag
			const ImGuiWindowFlags flags =
				ImGuiWindowFlags.Tooltip |
				ImGuiWindowFlags.NoDecoration |
				ImGuiWindowFlags.AlwaysAutoResize |
				ImGuiWindowFlags.NoSavedSettings |
				ImGuiWindowFlags.NoNav;

			ImGui.PushStyleVar( ImGuiStyleVar.WindowRounding, 0 );
			ImGui.PushStyleVar( ImGuiStyleVar.PopupBorderSize, 0 );
			ImGui.PushStyleVar( ImGuiStyleVar.WindowPadding, new Vector2( 8, 8 ) );

			if ( ImGui.Begin( string.Empty, flags ) )
			{
				var count = 0;
				foreach ( var command in Dev.Terminal.Find( _input ) )
				{
					// Only allow 8 Hints
					if ( count >= 8 )
					{
						break;
					}

					count++;

					if ( ImGui.Selectable( command.Member.Name ) )
					{
						_input = command.Member.Name;
					}

					if ( !string.IsNullOrWhiteSpace( command.Member.Help ) )
					{
						ImGui.SetTooltip( command.Member.Help );
					}

					if ( command.Parameters.Length <= 0 )
					{
						continue;
					}

					// Build Parameters GUI
					var stringBuilder = new StringBuilder();
					stringBuilder.Append( "[ " );

					for ( var i = 0; i < command.Parameters.Length; i++ )
					{
						if ( command.Info is MethodInfo method )
						{
							var parameter = method.GetParameters()[i];
							stringBuilder.Append( $"{parameter.ParameterType.Name} " );

							if ( parameter.HasDefaultValue )
							{
								stringBuilder.Append( $"= {parameter.DefaultValue ?? "Null"} " );
							}
						}
						else
						{
							var parameter = command.Parameters[i];
							stringBuilder.Append( $"{parameter.Name} " );
						}
					}

					stringBuilder.Append( "]" );

					ImGui.SameLine();
					ImGui.TextDisabled( stringBuilder.ToString() );
				}

				ImGui.End();
			}

			ImGui.PopStyleVar( 3 );
		}
	}
}
