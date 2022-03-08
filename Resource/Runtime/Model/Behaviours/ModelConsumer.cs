using UnityEngine;

namespace Espionage.Engine.Resources
{
	[DisallowMultipleComponent, Title( "Model" )]
	public class ModelConsumer : Component<Entity>
	{
		public Model Model
		{
			get => _model.Instances == 0 ? null : _model;
			set
			{
				_model?.Despawn( _modelOutput );
				_model = value;

				if ( _model != null )
				{
					_modelOutput = _model.Spawn( container );
				}
			}
		}

		// Private

		private Model _model;
		private GameObject _modelOutput;

		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );

			if ( !Files.Exists( modelPath ) )
			{
				Debugging.Log.Error( $"File [{Files.GetPath( modelPath )}], doesn't exist" );
				return;
			}

			_model = Model.Load( modelPath, () => Model.Spawn( container ) );
		}

		protected override void OnDelete()
		{
			_model.Despawn( _modelOutput );
		}

		// Fields

		[SerializeField]
		private Transform container;

		[SerializeField]
		private string modelPath;
	}
}
