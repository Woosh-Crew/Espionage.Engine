using System.Collections.Generic;
using ImGuiNET;
using ImPlotNET;
using UnityEngine;

namespace Espionage.Engine.Tools
{
	public class FramerateStats : Window
	{
		private readonly Queue<float> _fps = new( 20 );
		private int _lastFrame;
		private TimeSince _timeSinceUpdate = 0;

		private int _low;
		private int _top;

		public override void OnLayout()
		{
			// Using a queue is stupid, but i dont care

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

				if ( _fps.Count > 20 )
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

			ImGui.SetNextItemWidth( 256 );
			ImGui.PlotLines( string.Empty, ref values[0], _fps.Count - 1, 0, string.Empty, _low, _top, new( 0, 32 ) );
		}
	}
}
