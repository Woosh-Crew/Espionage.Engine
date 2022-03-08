using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[DisallowMultipleComponent, Title( "Model" )]
	public class ModelConsumer : Component<Entity>
	{
		public Model Model
		{
			get => _model?.Instances == 0 ? null : _model;
			set
			{

				_model?.Destroy( _modelOutput );
				_model = value;

				if ( _model != null )
				{
					_modelOutput = _model.Spawn( container );
				}
			}
		}

		public Transform Container => container;
		public string Path => modelPath;

		// Private

		private GameObject _modelOutput;
		private Model _model;

		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );
			Model = Model.Load( modelPath );
		}

		protected override void OnDelete()
		{
			_model?.Destroy( _modelOutput );
		}

		// Fields

		[SerializeField]
		private Transform container;

		[SerializeField]
		private string modelPath;
	}
}
