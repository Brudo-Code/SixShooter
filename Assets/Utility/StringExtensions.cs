public static class StringExtensions
{
	public static string FormatWithZeros(this string str, ref char[] buffer,int value)
	{
		string currentValueString = value.ToString();
		
		if(currentValueString.Length > buffer.Length)
		{
			currentValueString = currentValueString.Substring(0,buffer.Length);
		}

		for(int i = buffer.Length - 1; i >= 0; i--)
		{
			buffer[buffer.Length - i - 1] = i < currentValueString.Length ?
																  currentValueString.ToCharArray()[currentValueString.Length - i - 1] :
																  '0';
		}

		string finalString = new string(buffer);
		return finalString;
	}
}
