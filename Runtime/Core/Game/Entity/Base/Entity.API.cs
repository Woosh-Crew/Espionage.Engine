namespace Espionage.Engine
{
	public partial class Entity
	{
		/// <summary> All the entities that exists in the game world. </summary>
		public static Entities All { get; } = new();

		/// <summary> Create an Entity, from its type. </summary>
		public static T Create<T>() where T : Entity, new()
		{
			return Create( typeof( T ) ) as T;
		}

		/// <summary>
		/// Create an Entity, from its Library. behind the scene's it'll
		/// call a Library.Create using the fed in library and call the
		/// entities spawn method, as its implied it was called from code.
		/// If you don't it to call that, use Library.Create instead.
		/// Plus because its a library you can use the implicit operator for string
		/// to library.
		/// </summary>
		public static Entity Create( Library lib )
		{
			var ent =  (Entity)Library.Create( lib );
			ent.Spawn();
			return ent;
		}

		internal static Entity Constructor( Library lib )
		{ 
			var ent = (Entity)CreateInstance( lib.Info );
			return ent;
		}
	}
}
