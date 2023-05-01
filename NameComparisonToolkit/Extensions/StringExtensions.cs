namespace NameComparisonToolkit.Extensions;
public static class StringExtensions
{
	public static string Join(this IEnumerable<string> source, string separator)
		=> string.Join(separator, source);

	public static bool IsNullOrWhiteSpace(this string value)
		=> string.IsNullOrWhiteSpace(value);
}
