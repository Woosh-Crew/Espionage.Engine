using System.Linq;
using System.Text;

namespace Espionage.Engine
{
	public static class EString
	{
		public static int Hash( this string value )
		{
			var num1 = Encoding.Unicode.GetBytes( value ).Aggregate( 2166136261, ( current, num2 ) => (current ^ num2) * 16777619U );
			return (int)num1;
		}
	}
}
