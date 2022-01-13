namespace Espionage.Engine
{
	public sealed class OrderAttribute : Library.Component
	{
		public OrderAttribute( int order )
		{
			_order = order;
		}

		private int _order;
		public int Order => _order;
	}
}
