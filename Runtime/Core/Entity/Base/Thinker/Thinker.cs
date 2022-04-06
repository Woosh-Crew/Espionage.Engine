using System;
using System.Collections.Generic;

namespace Espionage.Engine
{
	public class Thinker
	{
		private Group _active;
		private bool _insideScope;
		private List<Group> _groups;

		public float Tick
		{
			set
			{
				if ( !_insideScope )
				{
					Dev.Log.Error( "Can't change tick, not inside think scope." );
					return;
				}

				_active.tick = value;
				_active.timeSinceLastThink = 0;
			}
		}

		public float Delta
		{
			get
			{
				if ( _insideScope )
				{
					return _active.timeSinceLastThink;
				}

				Dev.Log.Error( "Can't change tick, not inside think scope." );
				return 0;

			}
		}

		internal void Run()
		{
			if ( _groups == null )
			{
				return;
			}

			var changed = false;

			for ( var index = 0; index < _groups.Count; index++ )
			{
				// If we can't think, don't think.
				if ( _groups[index].tick <= 0 )
				{
					_groups.RemoveAt( index );
					Dev.Log.Info( "Removing Group, since it was 0" );

					changed = true;
					continue;
				}

				var timeSince = _groups[index].timeSinceLastThink;

				if ( !(timeSince > _groups[index].tick) )
				{
					continue;
				}

				_active = _groups[index];
				_active.tick = 0;

				// Checking for tick
				_insideScope = true;
				_active.method?.Invoke();
				_insideScope = false;

				_active.timeSinceLastThink = 0;

				_active = default;
			}

			if ( changed )
			{
				_groups.TrimExcess();
			}
		}

		public void Add( Action method, float tick )
		{
			_groups ??= new();

			_groups.Add( new()
			{
				method = method,
				timeSinceLastThink = 0,
				tick = tick
			} );
		}

		public void Remove( Action method )
		{
			var group = _groups.Find( e => e.method == method );

			if ( group != null )
			{
				_groups.Remove( group );
			}
		}

		private class Group
		{
			internal Action method;
			internal TimeSince timeSinceLastThink;
			internal float tick;
		}
	}
}
