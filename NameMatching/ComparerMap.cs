using NameMatching.Comparers;

namespace NameMatching;

public static class ComparerMap
{
	private static ComparerBase ExactMatchIgnoreCase { get; } = new ExactMatch();
	private static ComparerBase FirstNameLastNameIgnoreCase { get; } = new FirstLast();
	private static ComparerBase LastNameIgnoreCase { get; } = new Last();
	private static ComparerBase FirstLastSuffixIgnoreCase { get; } = new FirstLastSuffix();
	private static ComparerBase NoMatch { get; } = new NoMatch();

	public static ComparerBase GetComparer(this ComparisonType comparison)
		=> comparison switch
		{
			ComparisonType.ExactMatchIgnoreCase => ExactMatchIgnoreCase,
			ComparisonType.FirstNameLastNameIgnoreCase => FirstNameLastNameIgnoreCase,
			ComparisonType.LastNameIgnoreCase => LastNameIgnoreCase,
			ComparisonType.FirstLastSuffixIgnoreCase => FirstLastSuffixIgnoreCase,
			ComparisonType.NoMatch => NoMatch,
			_ => ExactMatchIgnoreCase,
		};
}
