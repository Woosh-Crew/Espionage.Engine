using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class Visuals : Component
	{
		public Visuals( Model model )
		{
			Model = model;
		}

		private Transform _root;

		public override void OnAttached( Entity item )
		{
			base.OnAttached( item );

			// Add Parent to Transform
			var go = new GameObject( "Visuals" );
			go.transform.SetParent( Entity );
			go.transform.localPosition = Vector3.zero;

			_root = go.transform;
		}

		// Utility

		public Animator Animator { get; private set; }
		public Renderer[] Renderers { get; private set; }

		// Model

		private Model _model;

		public Model Model
		{
			get
			{
				if ( _model?.Instances <= 0 )
				{
					_model = null;
					return null;
				}

				return _model;
			}
			set
			{
				// Apply Changes to Object
				_model = value;
				// Clone Object.
				Animator = null;
				Renderers = null;
			}
		}
	}
}
