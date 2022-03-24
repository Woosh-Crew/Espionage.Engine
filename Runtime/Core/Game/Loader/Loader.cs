using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// The loader is a callback based
	/// sequential loader, that has a UI
	/// representing its progress / status.
	/// </para>
	/// <para>
	/// Loader asks for a <see cref="ILoadable"/>
	/// which is an interface that tell the loader
	/// it can be loaded. (Like you probably guessed)
	/// </para>
	/// </summary>
	[Spawnable, Group( "Engine" )]
	public class Loader : ILibrary
	{
		public static void Start( params ILoadable[] request )
		{
			// We're in editor, don't do anything...
			if ( Engine.Game == null )
			{
				Dev.Log.Warning( "No Game is active." );
				return;
			}

			// Build Stack
			var stack = new Stack<ILoadable>();

			for ( var i = request.Length - 1; i >= 0; i-- )
			{
				stack.Push( request[i] );
			}

			Engine.Game.Loader.Start( stack );
		}

		//
		// Instance
		//

		public Library ClassInfo { get; }

		public Loader()
		{
			ClassInfo = Library.Register( this );
		}

		public Action Started { get; set; }
		public Action Finished { get; set; }

		// Debug

		public Stopwatch Timing { get; private set; }

		// Current

		public ILoadable Current { get; private set; }
		public float Progress => Stack.Count / Amount + Current.Progress / Amount;

		// States

		public void Start( Stack<ILoadable> request )
		{
			Assert.IsEmpty( request );

			if ( Stack != null )
			{
				throw new ApplicationException( "Already loading something" );
			}

			Timing = Stopwatch.StartNew();

			// Build Queue
			Stack = request;
			Amount = Stack.Count;

			// Start Loading
			Load();

			OnStart();
			Started?.Invoke();
		}

		protected virtual void OnStart() { }

		private void Finish()
		{
			OnFinish();
			Timing.Stop();
			Finished?.Invoke();

			Finished = null;
			Started = null;

			Stack = null;
			Current = null;
			Amount = 0;
		}

		protected virtual void OnFinish() { }

		// Queue

		private Stack<ILoadable> Stack { get; set; }
		private int Amount { get; set; }

		// Loading

		private void Load()
		{
			var possible = Stack.Peek();

			// Peek, see if we should inject
			var injection = possible.Inject();

			if ( injection is { Length: > 0 } )
			{
				for ( var i = injection.Length - 1; i >= 0; i-- )
				{
					Stack.Push( injection[i] );
				}

				Load();
			}

			// If nothing to inject, start loading
			Loading( Stack.Pop() );
		}

		private void Loading( ILoadable loadable )
		{
			if ( loadable == null )
			{
				OnLoad();
				return;
			}

			Current = loadable;
			loadable?.Load( OnLoad );
		}

		private void OnLoad()
		{
			if ( Stack.Count == 0 )
			{
				Finish();
				return;
			}

			Load();
		}
	}
}
