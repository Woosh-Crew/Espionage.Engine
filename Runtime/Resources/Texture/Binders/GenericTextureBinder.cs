using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Espionage.Engine.Resources.Binders
{
	public class GenericTextureBinder : Texture.Binder
	{
		public override string Identifier { get; }
		public override Texture2D Texture { get; set; }

		public GenericTextureBinder( string path, byte[] data )
		{
			Identifier = path;
			_data = data;
		}

		//
		// Data
		//

		private Texture2D _texture;
		private byte[] _data;
		
		public override void Load( Action<Texture2D> onLoad = null )
		{
			var texture = new Texture2D( 2, 2 );
			texture.LoadImage( _data );
			onLoad?.Invoke( texture );
		}

		public override void Unload( Action onUnload = null )
		{
			Object.Destroy( _texture );
			_data = null;
		}
	}
}
