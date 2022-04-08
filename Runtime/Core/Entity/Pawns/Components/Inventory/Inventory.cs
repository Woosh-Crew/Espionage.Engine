using System;
using System.Collections.Generic;

namespace Espionage.Engine
{
	public class Inventory : Component<Actor>
	{
		public IEnumerable<Pickup> All => Items;
		public int Count => Items.Count;

		private List<Pickup> Items { get; } = new();

		public virtual bool Add( Pickup item )
		{
			// If we dont own this and if we cant carry it, ignore
			if ( Contains( item ) || item.Carrier is not null )
			{
				Dev.Log.Info( $"Can't pickup item {item}" );
				return false;
			}

			Items.Add( item );
			item.OnPickup( Entity );

			return true;
		}

		public virtual bool Drop( Pickup item )
		{
			if ( !Contains( item ) )
			{
				Dev.Log.Error( $"Item [{item.ClassInfo.Name}] isn't in Inventory" );
				return false;
			}

			item.OnDrop( Entity );
			return Items.Remove( item );
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
	}
}
