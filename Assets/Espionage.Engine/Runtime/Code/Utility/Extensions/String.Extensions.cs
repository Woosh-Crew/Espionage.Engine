using System;

public static class StringExtensions
{
	// Copied from this (https://stackoverflow.com/a/2132004)
	public static string[] SplitArguments( this string commandLine )
	{
		var parmChars = commandLine.ToCharArray();
		var inSingleQuote = false;
		var inDoubleQuote = false;
		for ( var index = 0; index < parmChars.Length; index++ )
		{
			if ( parmChars[index] == '"' && !inSingleQuote )
			{
				inDoubleQuote = !inDoubleQuote;
				parmChars[index] = '\n';
			}
			if ( parmChars[index] == '\'' && !inDoubleQuote )
			{
				inSingleQuote = !inSingleQuote;
				parmChars[index] = '\n';
			}
			if ( !inSingleQuote && !inDoubleQuote && parmChars[index] == ' ' )
				parmChars[index] = '\n';
		}
		return (new string( parmChars )).Split( new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries );
	}
}
