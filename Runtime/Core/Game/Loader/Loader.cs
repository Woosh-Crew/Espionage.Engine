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
	[Spawnable, Singleton, Group( "Engine" )]
	public class Loader : ILibrary
	{
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

		public void Start( params ILoadable[] request )
		{
			Assert.IsEmpty( request );
			Assert.IsNotNull( Stack, "Already loading something" );

			Timing = Stopwatch.StartNew();

			// Build Queue
			Stack = Build( request );
			Amount = Stack.Count;

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

			Stack = null;
			Current = null;
			Amount = 0;
		}

		protected virtual void OnFinish() { }

		// Stack

		private static Stack<Request> Build( ILoadable[] requests )
		{
			// Build Stack
			var stack = new Stack<Request>();

			for ( var i = requests.Length - 1; i >= 0; i-- )
			{
				stack.Push( new( requests[i] ) );
			}

			return stack;
		}

		private Stack<Request> Stack { get; set; }
		private int Amount { get; set; }

		// Loading

		private void Load()
		{
			while ( true )
			{
				var possible = Stack.Peek();

				// We've injected, just load this
				if ( possible.Injected )
				{
					Loading( Stack.Pop().Loadable );
					return;
				}

				// Peek, see if we should inject
				var injection = possible.Loadable.Inject();

				if ( injection is { Length: > 0 } )
				{
					for ( var i = injection.Length - 1; i >= 0; i-- )
					{
						Stack.Push( new( injection[i] ) );
					}

					possible.Injected = true;

					// Rerun function, to see if injected
					// have more injections. injection inception..
					continue;
				}

				// If nothing to inject, start loading
				Loading( Stack.Pop().Loadable );
				break;
			}
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

		private class Request
		{
			public bool Injected { get; set; }
			public ILoadable Loadable { get; }

			public Request( ILoadable request )
			{
				Loadable = request;
				Injected = false;
			}
		}
	}
}
