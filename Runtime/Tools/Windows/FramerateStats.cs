using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class FramerateStats : Window
	{
		private readonly Queue<float> _fps = new( 40 );
		private int _lastFrame;
		private TimeSince _timeSinceUpdate = 0;

		private int _low;
		private int _top;

		public override void OnLayout()
		{
			// Stupid.. Yes
			ImGui.SetWindowSize( new( ImGui.GetWindowWidth(), 96 ), ImGuiCond.Always );

			if ( _timeSinceUpdate > 0.1f )
			{
				_timeSinceUpdate = 0;

				var value = 1 / Time.smoothDeltaTime;

				_fps.Enqueue( value );
				_lastFrame = (int)value;

				if ( value > _top )
				{
					_top = (int)value;
				}

				if ( value < _low || _low == 0 )
				{
					_low = (int)value;
				}

				if ( _fps.Count > 40 )
				{
					_fps.Dequeue();
				}
			}

			ImGui.Text( $"FPS: {_lastFrame}" );

			if ( _fps.Count <= 0 )
			{
				return;
			}

			var values = _fps.ToArray();

			ImGui.SetNextItemWidth( ImGui.GetWindowWidth() - 96 );
			ImGui.BeginGroup();
			{
				ImGui.PlotLines( string.Empty, ref values[0], _fps.Count - 1, 0, string.Empty, _low, _top, new( 0, 32 ) );
				ImGui.SameLine();
				ImGui.BeginGroup();
				{
					ImGui.Text( $"High: {_top}" );
					ImGui.Text( $"Low: {_low}" );
				}
				ImGui.EndGroup();
			}
			ImGui.EndGroup();
		}
	}
}
