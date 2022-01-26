using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Espionage.Engine.Resources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Espionage.Engine.Languages
{
	[Serializable, Library, Group( "Languages" ), File( Extension = ".lng", Serialization = "json" )]
	public class Language : IDisposable, IResource, IAsset
	{
		public string Name => name;
		public IReadOnlyDictionary<string, string> Localisation => localisation.ToDictionary( e => e.id, e => e.text );

		// Serialization
		[SerializeField]
		private string name;

		[SerializeField]
		private List<Group> localisation;

		[Serializable]
		private struct Group
		{
			public string id;
			public string text;
		}

		//
		// Instance
		//

		public Language( string path )
		{
			if ( !File.Exists( path ) )
			{
				Debugging.Log.Error( "Invalid Language Path" );
				throw new DirectoryNotFoundException();
			}

			Path = path;
		}

		public void Dispose()
		{
			Unload();
		}

		//
		// Resource
		//

		public string Path { get; }
		public bool IsLoading { get; private set; }

		public bool Load( Action onLoad = null )
		{
			IsLoading = true;

			using ( Debugging.Stopwatch( "Language Loaded", 5 ) )
			{
				var language = JsonUtility.FromJson<Language>( Path );
				name = language.name;
				localisation = language.localisation;
				Languages.Localisation.Database.Add( this );
			}

			IsLoading = false;

			return true;
		}

		public bool Unload( Action onUnload = null )
		{
			name = null;
			localisation = null;
			Languages.Localisation.Database.Remove( this );

			return true;
		}
	}
}
