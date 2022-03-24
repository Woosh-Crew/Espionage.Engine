using System;

namespace Espionage.Engine
{
	public class Operation : ILoadable
	{
		private readonly Action<Action> _operation;
		private readonly string _text;

		public static ILoadable Create( Action<Action> loaded, string text )
		{
			return new Operation( loaded, text );
		}

		private Operation( Action<Action> loaded, string text )
		{
			_text = text;
			_operation = loaded;
		}

		// Loadable

		float ILoadable.Progress => 0;
		string ILoadable.Text => _text;

		void ILoadable.Load( Action loaded )
		{
			_operation.Invoke( loaded );
		}
	}
}
