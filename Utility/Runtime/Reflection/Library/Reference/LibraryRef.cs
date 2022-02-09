using System;
using UnityEngine;

namespace Espionage.Engine
{
	[Serializable]
	public class LibraryRef
	{
		[SerializeField]
		private string identifier;

		public static implicit operator Library( LibraryRef  libraryRef )
		{
			return Library.Database[libraryRef.identifier];
		}
	}
}
