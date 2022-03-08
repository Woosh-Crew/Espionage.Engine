using UnityEngine;

namespace Espionage.Engine.Resources
{
	[DisallowMultipleComponent, Title( "Model" )]
	public class ModelConsumer : Component<Entity>
	{
		public Model Model
		{
			get => _model;
			set
			{
				_model.Despawn( _modelOutput );
				_model = value;
				_modelOutput  = _model.Spawn( container );
			}
		}
		
		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );

			if ( !Files.Exists( modelPath ) )
			{
				Debugging.Log.Error($"File [{modelPath}], doesn't exist");
				return;
			}

			Model = Model.Load( modelPath );
		}

		private Model _model;
		private GameObject _modelOutput;

		// Fields

		[SerializeField]
		private Transform container;

		[SerializeField]
		private string modelPath;
	}
}
