using System;
using UnityEngine;

namespace Espionage.Engine.Resources
{
	[Singleton]
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
			Animator = null;
			Renderers = null;

			_model?.Delete();
			_model = null;
			
			base.OnDetached();
		}

		// Utility

		public bool Enabled
		{
			get => _root.gameObject.activeSelf;
			set => _root.gameObject.SetActive( value );
		}

		public Action Changed { get; set; }
		public Animator Animator { get; private set; }
		public Renderer[] Renderers { get; private set; }
		public Bounds Bounds { get; private set; }

		// Model

		private Model.Instance _model;

		public Model Model
		{
			get => _model?.Model;
			set
			{
				if ( value == null )
				{
					_model?.Delete();
					_model = null;
					return;
				}

				if ( _model?.Model == value )
				{
					Debugging.Log.Warning( "Trying to apply the same Model" );
				}

				_model?.Delete();
				_model = value.Consume( _root );

				if ( _model == null )
				{
					return;
				}

				Animator = _model.GameObject.GetComponent<Animator>();
				Renderers = _root.GetComponentsInChildren<Renderer>();
				Bounds = default;

				foreach ( var renderer in Renderers )
				{
					if ( Bounds == default )
					{
						Bounds = renderer.bounds;
						continue;
					}

					Bounds.Encapsulate( renderer.bounds );
				}

				Changed?.Invoke();
			}
		}
	}
}
