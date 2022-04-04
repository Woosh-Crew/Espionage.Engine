using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	public class Visuals : Component
	{
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

		public override void OnDetached()
		{
			base.OnDetached();

			Animator = null;
			Renderers = null;

			_model?.Delete();
			_model = null;
		}

		// Utility

		public Action Changed { get; set; }
		public Animator Animator { get; private set; }
		public Renderer[] Renderers { get; private set; }

		// Model

		private Model.Instance _model;

		public Model Model
		{
			get => _model.Model;
			set
			{
				if ( _model?.Model == value )
				{
					Dev.Log.Info( "Trying to apply the same Model" );
					return;
				}

				if ( value == null )
				{
					_model?.Delete();
					_model = null;
					return;
				}

				_model?.Delete();
				_model = value.Consume( _root );

				if ( _model == null )
				{
					return;
				}

				Animator = _model.GameObject.GetComponent<Animator>();
				Renderers = _root.GetComponentsInChildren<Renderer>();
				Changed?.Invoke();
			}
		}
	}
}
