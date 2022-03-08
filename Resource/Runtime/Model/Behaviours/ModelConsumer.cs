using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[DisallowMultipleComponent, Title( "Model" )]
	public class ModelConsumer : Component<Entity>
	{
		public GameObject Object { get; private set; }

		public Model Model
		{
			get => _model?.Instances == 0 ? null : _model;
			set
			{

				_model?.Remove( Object );
				_model = value;

				if ( _model != null && Application.isPlaying )
				{
					Object = _model.Spawn( container );
				}
			}
		}

		public Transform Container => container;
		public string Path => modelPath;

		// Private

		private Model _model;

		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );
			Model = Model.Load( modelPath );
		}

		protected override void OnDelete()
		{
			_model?.Remove( Object );
		}

		// Fields

		[SerializeField]
		private Transform container;

		[SerializeField]
		private string modelPath;
	}
}
