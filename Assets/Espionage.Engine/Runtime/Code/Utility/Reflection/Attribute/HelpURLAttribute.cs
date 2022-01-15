using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Espionage.Engine
{
	[AttributeUsage( AttributeTargets.Class, Inherited = true )]
	public sealed class HelpURLAttribute : Attribute, Library.IComponent
	{
		public Library Library { get; set; }

		public HelpURLAttribute( string url )
		{
			_url = url;
		}

		private string _url;
		public string URL => _url;
	}
}