using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public interface IResource
	{
		string Identifier { get; }
		bool Persistant { get; set; }
	}
}
