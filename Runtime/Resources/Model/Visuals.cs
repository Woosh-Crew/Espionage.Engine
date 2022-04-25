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
			Model = null;
			Animator = null;
			Renderers = null;

			if ( _root != null )
			{
				GameObject.Destroy( _root );
			}

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

		private Model _model;

		public Model Model
		{
			get => _model;
			set => Change( value );
		}

		private void Change( Model value )
		{
			if ( value == null )
			{
				_model?.Delete();
				_model = null;
				return;
			}

			if ( _model == value )
			{
				Debugging.Log.Warning( "Trying to apply the same Model" );
			}

			_model?.Delete();
			_model = value;

			if ( _model == null )
			{
				return;
			}

			// Parent to Root
			var gameObject = _model.Cache;
			gameObject.SetActive( true );
			gameObject.transform.parent = _root;
			gameObject.transform.localPosition = Vector3.zero;

			// Get Renderers and Animator
			Animator = gameObject.GetComponent<Animator>();
			Renderers = gameObject.GetComponentsInChildren<Renderer>();
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

			// Finished
			Changed?.Invoke();
		}
	}
}
