using System;
using System.Collections.Generic;
using Espionage.Engine.Components;

namespace Espionage.Engine
{
	public class Inventory : Behaviour, IComponent<Pawn>, IDatabase<Pickup>
	{
		public IEnumerable<Pickup> All => Items;
		public Pawn Pawn { get; private set; }

		void IComponent<Pawn>.OnAttached( Pawn item )
		{
			Pawn = item;
		}

		private List<Pickup> Items { get; } = new();

		public virtual void Add( Pickup item )
		{
			// If we dont own this and if we cant carry it, ignore
			if ( Contains( item ) || item.Owner is not null )
			{
				Debugging.Log.Info( $"Can't pickup item {item}" );
				return;
			}

			Items.Add( item );
			item.OnPickup( Pawn );
		}

		public virtual void Drop( Pickup item )
		{
			if ( !Contains( item ) )
			{
				throw new Exception( $"Item [{item.ClassInfo.Name}] isn't in Inventory" );
			}

			item.OnDrop( Pawn );
			Items.Remove( item );
		}

		public virtual void Remove( Pickup item )
		{
			if ( item is null )
			{
				throw new NullReferenceException( "Pickup was NULL" );
			}

			Items.Remove( item );
		}

		public void Clear()
		{
			for ( var i = 0; i < Items.Count; i++ )
			{
				Remove( Items[i] );
			}

			Items.Clear();
		}

		public virtual bool Contains( Pickup item )
		{
			return Items.Contains( item );
		}

		public int Count => Items.Count;
	}
}
