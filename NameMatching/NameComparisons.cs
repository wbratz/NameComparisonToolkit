using NameMatching.Comparers;

namespace NameMatching;

public sealed class NameComparison
{
	public static IEqualityComparer<Name> ExactMatchIgnoreCase { get; } = new ExactMatchComparer();
	public static IEqualityComparer<Name> FirstNameLastNameIgnoreCase { get; } = new FirstNameLastNameComparer();
	public static IEqualityComparer<Name> LastNameIgnoreCase { get; } = new LastNameComparer();
	public static IEqualityComparer<Name> FirstLastSuffixIgnoreCase { get; } =
		new FirstNameLastNameSuffixComparer();
	public static IEqualityComparer<Name> NoMatch { get; } = new NoMatchComparer();
}
