using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Components;
using UnityEngine;

namespace Espionage.Engine
{
	/// <summary>
	/// <para>
	/// The Library is Espionage.Engines string based Reflection System.
	/// </para>
	/// <para>
	/// Libraries are used for storing meta data on a type, this includes
	/// Title, Name, Group, Icons, etc. You can also add your own data
	/// using components.
	/// We can do a lotta cool and performant shit because of this.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed partial class Library
	{
		public Library( [NotNull] Type type )
		{
			Class = type;
			Name = type.FullName;

			// Components
			Components = new Components<Library>( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			var attributes = Class.GetCustomAttributes();
			foreach ( var item in attributes )
			{
				if ( item is IComponent<Library> library )
				{
					Components.Add( library );
				}
			}

			// Properties
			Properties = new InternalPropertyDatabase();

			// Get all Properties (Defined by the User)
			const BindingFlags propertyFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
			foreach ( var propertyInfo in Class.GetProperties( propertyFlags ) )
			{
				if ( !propertyInfo.IsDefined( typeof( PropertyAttribute ) ) )
				{
					continue;
				}

				// Only add property if it has the attribute
				var attribute = propertyInfo.GetCustomAttribute<PropertyAttribute>();
				Properties.Add( attribute.CreateRecord( this, propertyInfo ) );
			}

			// Functions
			Functions = new InternalFunctionDatabase();

			// Get all Properties (Defined by the User)
			const BindingFlags methodFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
			foreach ( var methodInfo in Class.GetMethods( methodFlags ) )
			{
				if ( !methodInfo.IsDefined( typeof( FunctionAttribute ) ) )
				{
					continue;
				}

				// Only add property if it has the attribute
				var attribute = methodInfo.GetCustomAttribute<FunctionAttribute>();
				Functions.Add( attribute.CreateRecord( this, methodInfo ) );
			}
		}

		// Meta
		[field : SerializeField]
		public string Name { get; set; }

		[field : SerializeField]
		public string Title { get; set; }

		[field : SerializeField]
		public string Group { get; set; }

		[field : SerializeField]
		public string Help { get; set; }

		[field : SerializeField]
		public bool Spawnable { get; set; } = true;

		// Owner & Identification
		public Type Class { get; }
		public Guid Id => GenerateID( $"{Group}/{Name}" );

		//
		// Properties
		// 

		/// <summary>
		/// All the properties on this library. These are the members
		/// that have the Property attribute on them. They are used for
		/// serialization.
		/// </summary>
		public IDatabase<Property, string> Properties { get; private set; }

		/// <summary>
		/// All the properties on this library. These are the members
		/// that have the Property attribute on them. They are used for
		/// serialization.
		/// </summary>
		public IDatabase<Function, string> Functions { get; private set; }

		private class InternalPropertyDatabase : IDatabase<Property, string>
		{
			private readonly Dictionary<string, Property> _all = new();
			public IEnumerable<Property> All => _all.Values;

			public Property this[ string key ] => _all.ContainsKey( key ) ? _all[key] : null;

			public void Add( Property item )
			{
				_all.Add( item.Name, item );
			}

			public bool Contains( Property item )
			{
				return _all.ContainsKey( item.Name );
			}

			public void Remove( Property item )
			{
				_all.Remove( item.Name );
			}

			public void Clear()
			{
				_all.Clear();
			}
		}

		private class InternalFunctionDatabase : IDatabase<Function, string>
		{
			private readonly Dictionary<string, Function> _all = new();
			public IEnumerable<Function> All => _all.Values;

			public Function this[ string key ] => _all.ContainsKey( key ) ? _all[key] : null;

			public void Add( Function item )
			{
				_all.Add( item.Name, item );
			}

			public bool Contains( Function item )
			{
				return _all.ContainsKey( item.Name );
			}

			public void Remove( Function item )
			{
				_all.Remove( item.Name );
			}

			public void Clear()
			{
				_all.Clear();
			}
		}


		//
		// Components
		//

		/// <summary>
		/// Components are added meta data onto that library, this can
		/// include icons, company, stylesheet, etc. They allow us
		/// to do some really crazy cool shit
		/// </summary>
		public Components<Library> Components { get; }
	}
}
