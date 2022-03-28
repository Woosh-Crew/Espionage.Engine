using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

namespace Espionage.Engine.Tools.Editors
{
	[Target( typeof( Entity ) )]
	internal class EntityEditor : Inspector.Editor
	{
		private IEnumerable<IGrouping<string, Property>> _grouping;

		public override void OnActive( ILibrary library )
		{
			_grouping = library.ClassInfo.Properties.All.OrderBy( e => e.Components.Get<OrderAttribute>()?.Order ?? 5 ).GroupBy( e => e.Group );
		}

		public override void OnLayout( ILibrary item )
		{
			ImGui.Text( "Entity" );
			ImGui.SameLine();
			ImGui.TextColored( Color.gray, $"[{item.ClassInfo.Name} / {item.ClassInfo.Group}]" );

			foreach ( var group in _grouping )
			{
				if ( ImGui.TreeNode( string.IsNullOrEmpty( group.Key ) ? "None" : group.Key ) )
				{
					foreach ( var property in group )
					{
						ImGui.SetNextItemWidth( 256 );
						ImGui.Text( property.Title );
						ImGui.SameLine();
						Inspector.PropertyGUI( property, item );
					}
				}
			}
		}
	}
}
