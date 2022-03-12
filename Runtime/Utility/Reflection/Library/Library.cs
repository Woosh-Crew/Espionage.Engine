using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Components;

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
		/// <summary>
		/// All static Properties and Functions can be found in the Globals Library
		/// database index. It is here for easy viewing
		/// </summary>
		public static Library Global => Database[typeof( Global )];

		/// <summary>
		/// Components are added meta data onto that library, this can
		/// include icons, company, stylesheet, etc. They allow us
		/// to do some really crazy cool shit
		/// </summary>
		public Components<Library> Components { get; }

		internal Library( [NotNull] Type type )
		{
			string BuildName( Type type )
			{
				var name = string.Concat( type.Name!.Select( x => char.IsUpper( x ) ? "_" + x : x.ToString() ) ).TrimStart( '_' );

				if ( string.IsNullOrEmpty( type.Namespace ) )
				{
					return name.ToLower();
				}

				var prefix = type.Namespace?.Split( '.' )[^1] ?? "";
				return $"{prefix}.{name}".ToLower();
			}

			Class = type;
			Spawnable = true;

			Name = BuildName( type );

			// Components
			Components = new( this );

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

			// Grab Class Members

			const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

			// Properties

			Properties = new MemberDatabase<Property>();

			foreach ( var propertyInfo in Class.GetProperties( flags ) )
			{
				if ( !propertyInfo.IsDefined( typeof( PropertyAttribute ) ) )
				{
					continue;
				}

				var isStatic = propertyInfo.GetMethod.IsStatic;
				var attribute = propertyInfo.GetCustomAttribute<PropertyAttribute>();

				if ( isStatic )
				{
					Global.Properties.Add( attribute.CreateRecord( Global, propertyInfo ) );
				}
				else
				{
					// Only add property if it has the attribute
					Properties.Add( attribute.CreateRecord( this, propertyInfo ) );
				}
			}

			// Functions

			Functions = new MemberDatabase<Function>();

			foreach ( var methodInfo in Class.GetMethods( flags ) )
			{
				if ( !methodInfo.IsDefined( typeof( FunctionAttribute ) ) )
				{
					continue;
				}

				// Only add property if it has the attribute
				var attribute = methodInfo.GetCustomAttribute<FunctionAttribute>();

				if ( methodInfo.IsStatic )
				{
					Global.Functions.Add( attribute.CreateRecord( Global, methodInfo ) );
				}
				else
				{
					Functions.Add( attribute.CreateRecord( this, methodInfo ) );
				}
			}
		}

		// Meta
		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }
		public bool Spawnable { get; set; }

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
		/// All the functions on this library. These are the members
		/// that have the Function attribute on them. 
		/// </summary>
		public IDatabase<Function, string> Functions { get; private set; }

		private class MemberDatabase<T> : IDatabase<T, string> where T : class, IMember
		{
			private readonly Dictionary<string, T> _all = new();
			public IEnumerable<T> All => _all.Values;

			public T this[ string key ] => _all.ContainsKey( key ) ? _all[key] : null;

			public void Add( T item )
			{
				if ( _all.ContainsKey( item.Name ) )
				{
					Debugging.Log.Warning( $"Replacing {item.Name}" );
					_all[item.Name] = item;

					return;
				}

				_all.Add( item.Name, item );
			}

			public bool Contains( T item )
			{
				return _all.ContainsKey( item.Name );
			}

			public void Remove( T item )
			{
				_all.Remove( item.Name );
			}

			public void Clear()
			{
				_all.Clear();
			}

			public int Count => _all.Count;
		}
	}
}
