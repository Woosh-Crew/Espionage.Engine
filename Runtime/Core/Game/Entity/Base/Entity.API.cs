namespace Espionage.Engine
{
	public abstract partial class Entity
	{
		/// <summary> All the entities that exists in the game world. </summary>
		public static Entities All { get; } = new();

		/// <summary> Create an Entity, from its type. </summary>
		public static T Create<T>() where T : Entity, new()
		{
			return Library.Database.Create<T>();
		}

		/// <summary>
		/// Create an Entity, from its Library, behind the scene's it'll
		/// call a Library.Create using the fed in library. Plus because
		/// its a library you can use the implicit operator for string
		/// to library
		/// </summary>
		public static Entity Create( Library lib )
		{
			return Library.Create( lib ) as Entity;
		}
	}
}
