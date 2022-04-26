using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Espionage.Engine
{
	public class Threading : Module
	{
		public static Meta Main { get; private set; }
		public static Thread Current => Thread.CurrentThread;
		public static IDatabase<Meta, string> Running { get; } = new InternalDatabase();

		public static Meta Create( string name, Thread thread )
		{
			var meta = new Meta( name, thread );
			Running.Add( meta );

			if ( !thread.IsAlive )
			{
				thread.Start();
			}

			return meta;
		}

		// Main Thread 

		protected override void OnReady()
		{
			// Add Main Thread
			Main = Create( "main", Thread.CurrentThread );
		}

		protected override void OnUpdate()
		{
			Main.Run();
		}

		// Classes

		public class Meta
		{
			internal Meta( string name, Thread thread )
			{
				Name = name;
				Thread = thread;
			}

			public string Name { get; }
			public bool Closed { get; set; }
			public Thread Thread { get; }

			public void Enqueue( Action method )
			{
				Queue.Enqueue( method );
			}

			public void Close()
			{
				Closed = true;

				if ( Thread.CurrentThread != Thread )
				{
					// Maybe stupid?
					Thread.Abort();
				}

				Queue.Clear();
				Running.Remove( this );
			}

			public void Run()
			{
				if ( Closed )
				{
					return;
				}

				for ( var i = 0; i < Queue.Count; i++ )
				{
					Queue.Dequeue()?.Invoke();
				}
			}

			private Queue<Action> Queue { get; } = new();
		}

		private class InternalDatabase : IDatabase<Meta, string>
		{
			private readonly Dictionary<string, Meta> _storage = new( StringComparer.CurrentCultureIgnoreCase );

			public Meta this[ string key ] => _storage.ContainsKey( key ) ? _storage[key] : null;
			public int Count => _storage.Count;

			// API

			public void Add( Meta item )
			{
				_storage.Add( item.Name, item );

				Debugging.Log.Info( $"Adding Thread Meta {item.Name}" );
			}

			public bool Contains( Meta item )
			{
				return _storage.ContainsKey( item.Name );
			}

			public void Remove( Meta item )
			{
				item.Closed = true;
				_storage.Remove( item.Name );

				Debugging.Log.Info( $"Removing Thread Meta {item.Name}" );
			}

			public void Clear()
			{
				_storage.Clear();
			}

			// Enumerator

			public IEnumerator<Meta> GetEnumerator()
			{
				return _storage.Values.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}
