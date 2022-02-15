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
		public Library ClassInfo { get; }

		public Loader( string scene )
		{
			ClassInfo = Library.Database[GetType()];
			Scene = scene;
		}

		public Action Started { get; set; }
		public Action Finished { get; set; }
		public string Scene { get; }

		// Queue

		public bool IsLoading => Queue != null;
		public Queue<ILoadable> Queue { get; private set; }

		public virtual void Start( Queue<ILoadable> queue, Action onFinish = null )
		{
			if ( IsLoading )
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
		}

		private void Load( ILoadable loadable )
		{
			loadable?.Load( () => { Load( Queue.Dequeue() ); } );
		}
	}
}
