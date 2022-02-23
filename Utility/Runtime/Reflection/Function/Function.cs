using System.Reflection;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public sealed class Function : IMember
	{ 
		public MethodInfo Info { get; }
		public Components<Function> Components { get; }
		
		internal Function( Library owner, MethodInfo info )
		{
			ClassInfo = owner;
			Info = info;

			Name = info.Name;
			Title = info.Name;

			// Components
			Components = new Components<Function>( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			foreach ( var item in Info.GetCustomAttributes() )
			{
				if ( item is IComponent<Function> property )
				{
					Components.Add( property );
				}
			}
		}
		
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		
		private Library ClassInfo { get; }
	}
}
