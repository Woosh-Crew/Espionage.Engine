using System;

public static class StringExtensions
{
	// Copied from this (https://stackoverflow.com/a/2132004)
	public static string[] SplitArguments( this string commandLine )
	{
		var paramChars = commandLine.ToCharArray();
		var inSingleQuote = false;
		var inDoubleQuote = false;
		for ( var index = 0; index < paramChars.Length; index++ )
		{
			if ( paramChars[index] == '"' && !inSingleQuote )
			{
				inDoubleQuote = !inDoubleQuote;
				paramChars[index] = '\n';
			}

			if ( paramChars[index] == '\'' && !inDoubleQuote )
			{
				inSingleQuote = !inSingleQuote;
				paramChars[index] = '\n';
			}

			if ( !inSingleQuote && !inDoubleQuote && paramChars[index] == ' ' )
			{
				paramChars[index] = '\n';
			}
		}

		return new string( paramChars ).Split( new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries );
	}
}
