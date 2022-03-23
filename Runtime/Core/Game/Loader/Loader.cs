using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Assertions;

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
		public static void Start( params ILoadable[] request )
		{
			Engine.Game.Loader.Start( new( request ) );
		}

		//
		// Instance
		//

		public Action Started { get; set; }
		public Action Finished { get; set; }

		// Debug
		public Stopwatch Timing { get; private set; }

		// Queue

		public ILoadable Current { get; private set; }

		public void Start( Queue<ILoadable> queue, Action onFinish = null )
		{
			Assert.IsEmpty( queue );

			if ( Queue != null )
			{
				throw new ApplicationException( "Already loading something" );
			}

			Timing = Stopwatch.StartNew();

			Finished += () => Timing.Stop();
			Finished += onFinish;

			Queue = queue;

			// Start Loading
			Load( Queue.Dequeue() );

			OnStart();
			Started?.Invoke();
		}

		protected virtual void OnStart() { }

		private void Finish()
		{
			OnFinish();
			Finished?.Invoke();

			Finished = null;
			Started = null;

			Queue = null;
			Current = null;
		}

		protected virtual void OnFinish() { }

		private Queue<ILoadable> Queue { get; set; }

		private void Load( ILoadable loadable )
		{
			if ( loadable == null )
			{
				Dev.Log.Error( "Loader Quited early... To load was NULL" );

				Finish();
				return;
			}

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
