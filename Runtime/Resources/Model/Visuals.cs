using System.Xml.Schema;
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

		// Utility

		public Animator Animator { get; private set; }
		public Renderer[] Renderers { get; private set; }

		// Model

		private Model.Instance _model;

		public Model Model
		{
			get => _model.Model;
			set
			{
				if ( value == null )
				{
					return;
				}

				_model?.Delete();
				_model = value.Consume( _root );
			}
		}
	}
}
