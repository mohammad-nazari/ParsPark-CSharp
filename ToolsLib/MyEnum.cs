using System;

namespace ToolsLib
{
	public class MyEnum
	{
		public static T ParseEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}
	}
}
