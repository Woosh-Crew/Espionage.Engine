using System;

namespace Espionage.Engine.Resources
{
	public interface IModelProvider
	{
		// Id
		string Identifier { get; }
		
		// Outcome		
		
		// Loading Meta
		float Progress { get; }
		bool IsLoading { get; }

		// Resource
		void Load( Action finished );
		void Unload( Action finished );
	}
}
