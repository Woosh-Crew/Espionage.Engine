using System.IO;
using Espionage.Engine.Resources;
using UnityEngine;
using UnityEngine.UIElements;

namespace Espionage.Engine.Interfaces
{
	[RequireComponent( typeof(UIDocument) )]
	public abstract class GUI : Behaviour
	{
		private UIDocument _document;

		protected override void Awake()
		{
			base.Awake();
			_document = GetComponent<UIDocument>();
		}

		protected override void OnEnable()
		{
			if ( ClassInfo.Components.TryGet<FileAttribute>( out var file ) )
			{
				try
				{
					// Try loading the resource
					var resource = new Resource<Interface>( $"{file.Name}.ui" );
					resource.Load(() => _document.rootVisualElement.Add( OnCreateGUI(resource.Asset.asset.CloneTree())));
				}
				catch(DirectoryNotFoundException e)
				{
					Debugging.Log.Exception(e);
				}

				return;
			}
			
			_document.rootVisualElement.Add( OnCreateGUI(null) );
		}
		
		protected abstract VisualElement OnCreateGUI( VisualElement tree );
	}
}
