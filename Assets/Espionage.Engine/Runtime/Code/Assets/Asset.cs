using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine
{
	public abstract class Asset : ScriptableObject, ILibrary
	{
		public Library ClassInfo { get; set; }

		public Asset()
		{
			ClassInfo = Library.Database.Get( GetType() );
		}

		//
		// Editor
		//

#if UNITY_EDITOR

		/// <summary> Only used in Editor </summary>
		public abstract void Compile();

		/// <summary> GUI that shows in the compiler </summary>
		public virtual VisualElement CompilerGUI() { throw null; }

#endif
	}
}
