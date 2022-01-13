using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Espionage.Engine.Internal;

using Random = System.Random;

namespace Espionage.Engine
{
	public partial class Library
	{
		/// <summary> Database for library records </summary>
		public static IDatabase<Library> Database => _database;

		/// <summary> Attribute that skips the attached class from generating a library reference </summary>
		[AttributeUsage( AttributeTargets.Class, Inherited = true )]
		public class Skip : Attribute { }

		//
		// Components
		//

		[AttributeUsage( AttributeTargets.Class, Inherited = true )]
		public abstract class Component : Attribute
		{
			public Library Library { get; internal set; }

			public virtual void OnAttached() { }
		}

		/// <summary> Attribute that allows the definition of a custom constructor </summary>
		public sealed class Constructor : Component
		{
			/// <param name="constructor"> Method should return ILibrary </param>
			public Constructor( string constructor )
			{
				this.constructor = constructor;
			}

			private string constructor;

			public override void OnAttached()
			{
				base.OnAttached();

				// Works?
				UnityEngine.Debug.Log( $"Constructor Attached to Class {Library.Name}" );
			}
		}
	}
}
