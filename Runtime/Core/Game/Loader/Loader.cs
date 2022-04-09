using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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
		public float Progress => Current.Progress;

		// States

		public void Start( params ILoadable[] request )
		{
			var final = new Request[request.Length];

			for ( var i = 0; i < request.Length; i++ )
			{
				final[i] = new( request[i] );
			}

			Start( final );
		}

		public void Start( params Func<ILoadable>[] request )
		{
			var final = new Request[request.Length];

			for ( var i = 0; i < request.Length; i++ )
			{
				final[i] = new( request[i] );
			}

			Start( final );
		}

		public void Start( params Request[] request )
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

			Debugging.Log.Info( "Finished" );

			Stack = null;
			Current = null;
			Amount = 0;
		}

		protected virtual void OnFinish() { }

		// Stack

		private static Stack<Request> Build( Request[] requests )
		{
			// Build Stack
			var stack = new Stack<Request>();

			for ( var i = requests.Length - 1; i >= 0; i-- )
			{
				stack.Push( requests[i] );
			}

			return stack;
		}

		private Stack<Request> Stack { get; set; }
		private int Amount { get; set; }

		// Loading

		private void Load()
		{
			if ( Stack.Count == 0 )
			{
				Finish();
				return;
			}

			while ( true )
			{
				var possible = Stack.Peek();

				// If it doesn't exist, just continue
				if ( possible.Loadable == null )
				{
					Stack.Pop();
					continue;
				}

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
			Load();
		}

		public class Request
		{
			public bool Injected { get; internal set; }

			// Loadable

			private ILoadable _cached;

			public ILoadable Loadable
			{
				get
				{
					if ( _cached == null && Invoker == null )
					{
						return null;
					}

					return _cached ??= Invoker.Invoke();
				}
			}

			private Func<ILoadable> Invoker { get; }

			public Request( Func<ILoadable> request )
			{
				Invoker = request;
				Injected = false;
			}

			public Request( ILoadable request )
			{
				_cached = request;
				Injected = false;
			}
		}
	}
}
