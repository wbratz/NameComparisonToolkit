using NameComparisonToolkit.Comparers;

namespace NameComparisonToolkit;

internal static class ComparerMap
{
	private static ComparerBase ExactMatchIgnoreCase { get; } = new ExactMatch();
	private static ComparerBase FirstNameLastNameIgnoreCase { get; } = new FirstLast();
	private static ComparerBase LastNameIgnoreCase { get; } = new Last();
	private static ComparerBase FirstLastSuffixIgnoreCase { get; } = new FirstLastSuffix();

	public static ComparerBase GetComparer(this ComparisonType comparison)
		=> comparison switch
		{
			ComparisonType.ExactMatch => ExactMatchIgnoreCase,
			ComparisonType.FirstLast => FirstNameLastNameIgnoreCase,
			ComparisonType.Last => LastNameIgnoreCase,
			ComparisonType.FirstLastSuffix => FirstLastSuffixIgnoreCase,
			_ => ExactMatchIgnoreCase,
		};
}
