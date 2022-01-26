using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Espionage.Engine.Languages
{
	/// <summary>
	/// A Text is basically a string that contains a localised
	/// string, you feed it the Id of a localised string, and it
	/// will return a string. Very helpful!
	/// </summary>
	public class Text
	{
		private string _id;

		public Text( string id )
		{
			_id = id;
		}

		public static implicit operator string( Text text )
		{
			return Localisation.Current.Localisation[text._id];
		}
	}
}
