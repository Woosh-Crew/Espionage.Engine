using System;
using System.Collections.Generic;

namespace Espionage.Engine
{
	/// <summary>
	/// This is a class for managing the loading of Maps
	/// or loading into a network game. It'll load the
	/// target scene. From that its up to you to display
	/// loading progress and text.
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Loader
	{
		public Loader( string scene ) { }

		public Action Started { get; set; }
		public Action Finished { get; set; }

		// Queue

		public ILoadable Current { get; private set; }

		public virtual void Start( Queue<ILoadable> queue, Action onFinish = null )
		{
			if ( Queue != null )
			{
				throw new ApplicationException( "Already loading something" );
			}

			Queue = queue;
			Load( Queue.Dequeue() );

			Started?.Invoke();
			Finished += onFinish;
		}

		protected virtual void Finish()
		{
			Finished?.Invoke();

			Finished = null;
			Started = null;

			Queue = null;
			Current = null;
		}

		private Queue<ILoadable> Queue { get; set; }

		private void Load( ILoadable loadable )
		{
			Current = loadable;
			loadable?.Load( () =>
			{
				if ( Queue.Count == 0 )
				{
					Finish();
					return;
				}

				Load( Queue.Dequeue() );
			} );
		}
	}
}
