using UnityEngine;

namespace Espionage.Engine.Converters
{
	public class UnityConverter : IConverter<Vector2>, IConverter<Vector3>, IConverter<Vector4>
	{
		public Library ClassInfo { get; } = Library.Database[typeof( UnityConverter )];

		
		Vector2 IConverter<Vector2>.Convert( string value ) { }
		Vector3 IConverter<Vector3>.Convert( string value ) { }
		Vector4 IConverter<Vector4>.Convert( string value ) { }
	}
}
