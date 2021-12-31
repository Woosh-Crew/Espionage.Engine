using System;
using System.Collections.Generic;

namespace Espionage.Engine.Internal
{
	public interface ILoggingProvider
	{
		Action OnLogged { get; set; }

		void Add();
		void Clear();

		IReadOnlyCollection<string> All { get; }
	}
}
