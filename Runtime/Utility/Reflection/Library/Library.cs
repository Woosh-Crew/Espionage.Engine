using System;
using System.Collections.Generic;
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
	public sealed partial class Library : ILibrary, IMeta
	{
		/// <summary>
		/// This is here so we can reference a library like its a
		/// ILibrary object. You can loop this very easily, so make
		/// sure you don't do that... 
		/// </summary>
		public Library ClassInfo => this;

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
			Assert.IsNull( type );

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

			Info = type;
			Spawnable = true;

			Name = BuildName( type );

			// Components
			Components = new( this );

			// This is really expensive (6ms)...
			// Get Components attached to type
			var attributes = Info.GetCustomAttributes();
			foreach ( var item in attributes )
			{
				if ( item is IComponent<Library> library )
				{
					Components.Add( library );
				}
			}

			Properties = new MemberDatabase<Property, PropertyInfo>( this );
			Functions = new MemberDatabase<Function, MethodInfo>( this );

			foreach ( var info in Info.GetMembers( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic ) )
			{
				switch ( info )
				{
					case PropertyInfo prop :
						CacheProperty( prop );
						break;
					case MethodInfo method :
						CacheFunction( method );
						break;
				}
			}
		}

		private void CacheProperty( PropertyInfo info )
		{
			// If this property came from a class outside the scope of ILibrary
			// Ignore it. We don't care about it. 
			if ( !IsValid( info.DeclaringType ) || info.HasAttribute<SkipAttribute>( true ) )
			{
				return;
			}

			var reg = MemberDatabase<Property, PropertyInfo>.Registry;

			if ( reg.ContainsKey( info.DeclaringType ) )
			{
				var potential = reg[info.DeclaringType].FirstOrDefault( e => e.Name == info.Name );

				if ( potential != null )
				{
					Properties.Add( potential );
					return;
				}
			}

			if ( info.IsDefined( typeof( PropertyAttribute ) ) )
			{
				var isStatic = info.GetMethod?.IsStatic ?? info.SetMethod.IsStatic;
				(isStatic ? Global : this).Properties.Add( info.GetCustomAttribute<PropertyAttribute>().CreateRecord( info ) );

				return;
			}

			Properties.Add( new( info, info.Name, default ) );
		}

		private void CacheFunction( MethodInfo info )
		{
			if ( !info.IsDefined( typeof( FunctionAttribute ) ) || info.HasAttribute<SkipAttribute>( true ) )
			{
				return;
			}

			var reg = MemberDatabase<Function, MethodInfo>.Registry;

			if ( reg.ContainsKey( info.DeclaringType ) )
			{
				var potential = reg[info.DeclaringType].FirstOrDefault( e => e.Name == info.Name );

				if ( potential != null )
				{
					Functions.Add( potential );
					return;
				}
			}

			var attribute = info.GetCustomAttribute<FunctionAttribute>();
			(info.IsStatic ? Global : this).Functions.Add( attribute.CreateRecord( info ) );
		}

		//
		// Meta
		//

		public string Name { get; set; }
		public string Title { get; set; }
		public string Group { get; set; }
		public string Help { get; set; }

		public bool Spawnable { get; set; }

		// Owner & Identification
		public Type Info { get; }
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

		private class MemberDatabase<T, TInfo> : IDatabase<T, string> where T : class, IMember<TInfo> where TInfo : MemberInfo
		{
			internal static readonly Dictionary<Type, HashSet<T>> Registry = new();

			private readonly Dictionary<string, T> _all = new();
			public IEnumerable<T> All => _all.Values;

			private readonly Library _owner;

			public MemberDatabase( Library owner )
			{
				_owner = owner;
			}

			public T this[ string key ] => _all.ContainsKey( key ) ? _all[key] : null;

			public void Add( T item )
			{
				// Add the Key to the registry, if its null
				if ( !Registry.ContainsKey( item.Info.DeclaringType ) )
				{
					Registry.Add( item.Info.DeclaringType, new() );
				}

				// See if we can add it to the registry, so it prevents duplicate members
				if ( Registry[item.Info.DeclaringType].All( e => e.Name != item.Name ) )
				{
					Registry[item.Info.DeclaringType].Add( item );
				}

				// Assign Proper owner, if we're the owner.
				if ( item.Owner == null && item.Info.DeclaringType == _owner.Info )
				{
					item.Owner = _owner;

					if ( string.IsNullOrEmpty( item.Group ) )
					{
						item.Group = _owner.Title;
					}
				}

				// Now add it to the instance
				if ( _all.ContainsKey( item.Name ) )
				{
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
