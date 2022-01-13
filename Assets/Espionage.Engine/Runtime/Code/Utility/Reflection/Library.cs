using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	/// <summary> Espionage.Engines string based Reflection System </summary>
	[Serializable] // Instance Serialization
	public partial class Library
	{
		public string Name;
		public string Title;
		public string Description;
		public string Icon;

		public int Order;

		// Owner

		public Library WithOwner( Type type )
		{
			Owner = type;
			return this;
		}

		[NonSerialized]
		public Type Owner;

		// GUID

		public Library WithId( string name )
		{
			Id = GenerateID( name );
			return this;
		}

		[NonSerialized]
		public Guid Id;

		// Construtor

		public Library WithConstructor( ConstructorRef constructor )
		{
			_constructor = constructor;
			return this;
		}

		[NonSerialized]
		private ConstructorRef _constructor;
	}
}
