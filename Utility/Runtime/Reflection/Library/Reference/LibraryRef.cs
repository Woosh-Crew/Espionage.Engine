using System;
using UnityEngine;

namespace Espionage.Engine
{
	[Serializable]
	public class LibraryRef
	{
		[SerializeField]
		private string identifier;

		protected LibraryRef() { }

		public static implicit operator Library( LibraryRef libraryRef )
		{
			return Library.Database[libraryRef.identifier];
		}
	}
}
