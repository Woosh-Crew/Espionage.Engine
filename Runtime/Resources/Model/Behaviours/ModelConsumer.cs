using System;
using Espionage.Engine.Resources.Internal;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[DisallowMultipleComponent, Title( "Model" )]
	public class ModelConsumer : Component
	{
		public GameObject Object { get; private set; }

		public Model Model
		{
			get => _model?.Instances == 0 ? null : _model;
			set
			{
				_model?.Remove( Object );
				_model = value;

				if ( _model != null )
				{
					Object = _model.Spawn( Container );
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

			Model = Model.Load( Path );
		}

		// Fields

		[SerializeField]
		private Transform container;

		[SerializeField]
		private string modelPath;
	}
}
