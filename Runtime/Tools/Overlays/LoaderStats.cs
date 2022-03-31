using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using ImPlotNET;

namespace Espionage.Engine.Tools
{
	public class LoaderStats : Overlay
	{
		public override void OnLayout()
		{
			var loader = Engine.Game.Loader;

			if ( loader.Current == null )
			{
				ImGui.Text( "Nothing is being loaded." );

				// Last Loaded
				if ( loader.Timing != null )
				{
					ImGui.Separator();
					var time = loader.Timing.Elapsed.Seconds > 0 ? $"{loader.Timing.Elapsed.TotalSeconds} seconds" : $"{loader.Timing.Elapsed.TotalMilliseconds} ms";
					ImGui.Text( $"Load Time: {time}" );
				}

				return;
			}

			ImGui.Text( $"Text: {loader.Current.Text}" );
			ImGui.ProgressBar( loader.Current.Progress, new( 0, 0 ) );
		}
	}
}
