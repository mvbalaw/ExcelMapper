namespace ExcelMapper.Repository
{
	public static class StringExtensions
	{
		public static string GetClassName(this string value)
		{
			return value.Replace("$", "");
		}
	}
}