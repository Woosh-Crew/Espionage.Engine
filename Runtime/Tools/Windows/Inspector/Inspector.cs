using System;
using System.Collections.Generic;
using ImGuiNET;

namespace Espionage.Engine.Tools
{
	public sealed class Inspector : Window
	{
		public override void OnLayout()
		{
			// Don't render if there is nothing there.
			if ( Service.Selection == null )
			{
				ImGui.Text( "Nothing Selected!" );
				return;
			}

			HeaderGUI( Service.Selection );
			DrawGUI( Service.Selection );
		}

		public void SelectionChanged( object selection )
		{
			if ( !Editors.ContainsKey( selection.GetType() ) )
			{
				Editors.Add( selection.GetType(), GrabEditor( selection.GetType() ) );
			}

			Editors[selection.GetType()]?.OnActive( selection );
		}

		private void HeaderGUI( object selection )
		{
			if ( Editors.ContainsKey( selection.GetType() ) )
			{
				if ( Editors[selection.GetType()] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( selection.ToString() );
					Editors[selection.GetType()].OnHeader( selection );
					ImGui.PopID();

					ImGui.EndGroup();

					ImGui.Separator();
				}

				return;
			}

			// Get Editor, if we haven't already
			Editors.Add( selection.GetType(), GrabEditor( selection is Library ? typeof( Library ) : selection.GetType() ) );
		}

		private static void DrawGUI( object item )
		{
			if ( Editors.ContainsKey( item.GetType() ) )
			{
				if ( Editors[item.GetType()] != null )
				{
					ImGui.BeginGroup();

					ImGui.PushID( item.ToString() );
					Editors[item.GetType()].OnLayout( item );
					ImGui.PopID();

					ImGui.EndGroup();
				}

				return;
			}

			// Get Editor, if we haven't already
			Editors.Add( item.GetType(), GrabEditor( item is Library ? typeof( Library ) : item.GetType() ) );
		}

		public static void PropertyGUI( Property property, object instance )
		{
			if ( PropertyGUI( property.Type, property, instance, property[instance], out var changed ) )
			{
				property[instance] = changed;
			}
		}

		public static bool PropertyGUI( Type target, Property property, object instance, object value, out object changed )
		{
			if ( !Drawers.ContainsKey( target ) )
			{
				Drawers.Add( target, GrabDrawer( target ) );

				changed = null;
				return false;
			}

			if ( Drawers[target] == null )
			{
				ImGui.TextDisabled( value?.ToString() ?? "Null" );
				if ( ImGui.IsItemHovered() )
				{
					ImGui.SetTooltip( "Item is not inherited from ILibrary, or there is no valid drawer" );
				}

				changed = null;
				return false;
			}

			// Normal Drawer
			ImGui.BeginGroup();
			{
				ImGui.PushID( property?.Name ?? target.Name );

				var drawer = Drawers[target];
				drawer.Type = target;
				drawer.Property = property;

				drawer.OnLayout( instance, value, out changed );

				ImGui.PopID();
			}
			ImGui.EndGroup();

			if ( ImGui.IsItemHovered() && !string.IsNullOrWhiteSpace( property?.Help ) )
			{
				ImGui.SetTooltip( property.Help );
			}

			return changed != default;
		}

		private static Editor GrabEditor( Type type )
		{
			// See if we can use Type Bound.
			var lib = Library.Database.Find<Editor>( e => type == e.Components.Get<TargetAttribute>()?.Type );

			// Now see if we can use interface bound if its null.
			lib ??= Library.Database.Find<Editor>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();

				if ( comp == null )
				{
					return false;
				}

				return comp.Type.IsInterface && type.HasInterface( comp.Type );
			} );

			// Still NULL? See if we can find a inherited type
			lib ??= Library.Database.Find<Editor>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();
				return comp != null && type.IsSubclassOf( comp.Type );
			} );

			return lib == null ? null : Library.Database.Create<Editor>( lib.Info );
		}

		private static Drawer GrabDrawer( Type type )
		{
			if ( type.IsEnum )
			{
				// Explicit Enum Drawer
				return Library.Database.Create<EnumDrawer>();
			}

			if ( type.IsArray )
			{
				// Explicit Array Drawer
				return Library.Database.Create<ArrayDrawer>();
			}

			// See if we can use Type Bound.
			var lib = Library.Database.Find<Drawer>( e => type == e.Components.Get<TargetAttribute>()?.Type );

			// Now see if we can use interface bound if its null.
			lib ??= Library.Database.Find<Drawer>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();

				if ( comp == null )
				{
					return false;
				}

				return comp.Type.IsInterface && type.HasInterface( comp.Type );
			} );

			// Still NULL? See if we can find a inherited type
			lib ??= Library.Database.Find<Drawer>( e =>
			{
				var comp = e.Components.Get<TargetAttribute>();
				return comp != null && type.IsSubclassOf( comp.Type );
			} );

			if ( lib == null )
			{
				// See if we have one from ILibrary
				return type.HasInterface<ILibrary>() ? Library.Database.Create<ILibraryDrawer>() : null;
			}

			return Library.Database.Create<Drawer>( lib.Info );
		}

		//
		// UI
		// 

		private static readonly Dictionary<Type, Drawer> Drawers = new();
		private static readonly Dictionary<Type, Editor> Editors = new();

		[Singleton, Group( "Inspector" )]
		public abstract class Drawer : ILibrary
		{
			// Data

			public Type Type { get; internal set; }
			public Property Property { get; internal set; }

			// Instance

			public Library ClassInfo { get; }

			public Drawer()
			{
				ClassInfo = Library.Register( this );
			}

			/// <param name="property"> [Nullable] the property that owns this drawer</param>
			/// <param name="instance"> The current instance we're iterating through </param>
			/// <param name="value"> The current value of this property </param>
			/// <param name="change"> The value we should change to if this returns true. </param>
			/// <returns>True if the value of this property has changed</returns>
			public abstract bool OnLayout( object instance, in object value, out object change );
		}

		public abstract class Drawer<T> : Drawer
		{
			public override bool OnLayout( object instance, in object value, out object change )
			{
				// String has its own NULL stuff. This is hacky, but can't do much about that.
				if ( typeof( T ) != typeof( string ) && value == null )
				{
					ImGui.Text( "Null" );

					change = null;
					return false;
				}

				try
				{
					if ( OnLayout( instance, (T)value, out var changed ) )
					{
						change = changed;
						return true;
					}
				}
				catch ( InvalidCastException )
				{
					Dev.Log.Info( $"Tried casting {value.GetType()} to {typeof( T ).Name} " );
				}


				change = null;
				return false;
			}

			/// <inheritdoc cref="Drawer.OnLayout"/>
			protected abstract bool OnLayout( object instance, in T value, out T change );
		}

		[Singleton, Group( "Inspector" )]
		public abstract class Editor : ILibrary
		{
			public Library ClassInfo { get; }

			public Editor()
			{
				ClassInfo = Library.Register( this );
			}

			public virtual void OnActive( object item ) { }
			public virtual void OnHeader( object item ) { }

			public abstract void OnLayout( object instance );
		}

		public abstract class Editor<T> : Editor
		{
			public sealed override void OnActive( object item )
			{
				OnActive( (T)item );
			}

			public sealed override void OnHeader( object item )
			{
				OnHeader( (T)item );
			}

			public sealed override void OnLayout( object instance )
			{
				OnLayout( (T)instance );
			}

			protected virtual void OnActive( T item ) { }
			public virtual void OnHeader( T item ) { }
			protected abstract void OnLayout( T item );
		}
	}

}
