using NameMatching.Comparers;

namespace NameMatching;

public sealed class Comparison
{
	public static IEqualityComparer<Name> ExactMatchIgnoreCase { get; } = new ExactMatch();
	public static IEqualityComparer<Name> FirstNameLastNameIgnoreCase { get; } = new FirstLast();
	public static IEqualityComparer<Name> LastNameIgnoreCase { get; } = new Last();
	public static IEqualityComparer<Name> FirstLastSuffixIgnoreCase { get; } = new FirstLastSuffix();
	public static IEqualityComparer<Name> NoMatch { get; } = new NoMatch();
}
