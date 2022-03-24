using System;
using System.Threading.Tasks;

namespace Espionage.Engine
{
	public static class Operation
	{
		public static ILoadable Create( Action<Action> loaded, string text )
		{
			return new ActionBasedCallback( loaded, text );
		}

		public static ILoadable Create( Task loaded, string text )
		{
			var operation = new TaskBasedCallback( loaded, text );
			return operation;
		}

		private class ActionBasedCallback : ILoadable
		{
			private readonly Action<Action> _operation;

			public ActionBasedCallback( Action<Action> loaded, string text )
			{
				Text = text;
				_operation = loaded;
			}

			// Loadable

			public float Progress { get; }
			public string Text { get; }

			public void Load( Action loaded )
			{
				_operation.Invoke( loaded );
			}
		}

		private class TaskBasedCallback : ILoadable
		{
			private readonly Task _task;

			public TaskBasedCallback( Task loaded, string text )
			{
				Text = text;
				_task = loaded;
			}

			// Loadable

			public float Progress { get; set; }
			public string Text { get; }

			public async void Load( Action loaded )
			{
				try
				{
					await _task;
				}
				catch ( Exception e )
				{
					Dev.Log.Exception( e );
				}

				loaded.Invoke();
			}
		}
	}
}
