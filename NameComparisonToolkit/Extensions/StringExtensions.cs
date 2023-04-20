namespace NameComparisonToolkit.Extensions;
public static class StringExtensions
{
	public static string Join(this IEnumerable<string> source, string separator)
		=> source.Join(separator);

	public static bool IsNullOrWhiteSpace(this string value)
		=> string.IsNullOrWhiteSpace(value);
}
