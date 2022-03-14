using System;

namespace Espionage.Engine
{
	public interface ILoadable
	{
		float Progress { get; }
		string Text { get; }

		void Load( Action loaded = null );
	}
}
