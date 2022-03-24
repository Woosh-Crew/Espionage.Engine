using System;
using System.Threading.Tasks;

namespace Espionage.Engine
{
	public class Operation : ILoadable
	{
		private readonly Action<Action> _operation;
		private readonly string _text;
		private readonly Task _task;

		public static ILoadable Create( Action<Action> loaded, string text )
		{
			return new Operation( loaded, text );
		}

		public static ILoadable Create( Task loaded, string text )
		{
			return new Operation( loaded, text );
		}

		private Operation( Task loaded, string text )
		{
			_text = text;
			_task = loaded;
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
			if ( _task != null )
			{
				_task.ContinueWith( t => loaded.Invoke() );
				_task.Start();

				return;
			}

			_operation.Invoke( loaded );
		}
	}
}
