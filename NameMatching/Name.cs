namespace NameMatching;

public sealed class Name
{
	private static readonly string _ignoredString = "NONE";
	public string FirstName { get; }
	public string LastName { get; }
	public string MiddleName { get; }
	public string Suffix { get; }

	public string GetFullName(bool includeSuffix)
		=> string.Join(" ", new string[]
		{
			FirstName,
			MiddleName,
			LastName,
			includeSuffix ? Suffix : string.Empty
		}.Where(x => !string.IsNullOrEmpty(x)));

	public List<string> GetTokenizedName(bool includeSuffix)
		=> GetFullName(includeSuffix).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

	public Name()
	{
		FirstName = string.Empty;
		LastName = string.Empty;
		MiddleName = string.Empty;
		Suffix = string.Empty;
	}

	public Name(string firstName, string lastName)
	{
		FirstName = firstName?.Trim(' ') ?? string.Empty;
		LastName = lastName?.Trim(' ') ?? string.Empty;
		MiddleName = string.Empty;
		Suffix = string.Empty;
	}

	public Name(string firstName, string lastName, string middleName, string suffix)
	{
		FirstName = firstName?.Trim(' ') ?? string.Empty;
		LastName = lastName?.Trim(' ') ?? string.Empty;
		MiddleName = middleName == null ? string.Empty : middleName.Trim(' ');
		Suffix = ReplaceIgnoredStrings(suffix == null ? string.Empty : suffix.Trim(' '));
	}

	public bool Matches(Name name)
	{
		return Matches(name, NameComparison.ExactMatchIgnoreCase);
	}

	public bool Matches(Name name, NameComparisonType comparison)
	{
		return Matches(name, GetComparer(comparison));
	}

	public bool MatchesAny(IEnumerable<Name> names, NameComparisonType comparison)
	{
		var comparer = GetComparer(comparison);
		return names.Any(x => Matches(x, comparer));
	}

	private static IEqualityComparer<Name> GetComparer(NameComparisonType comparison)
	{
		return comparison switch
		{
			NameComparisonType.ExactMatchIgnoreCase => NameComparison.ExactMatchIgnoreCase,
			NameComparisonType.FirstNameLastNameIgnoreCase => NameComparison.FirstNameLastNameIgnoreCase,
			NameComparisonType.LastNameIgnoreCase => NameComparison.LastNameIgnoreCase,
			NameComparisonType.FirstLastSuffixIgnoreCase => NameComparison.FirstLastSuffixIgnoreCase,
			NameComparisonType.NoMatch => NameComparison.NoMatch,
			_ => NameComparison.ExactMatchIgnoreCase,
		};
	}

	public bool Matches(Name name, IEqualityComparer<Name> comparer)
	{
		return comparer.Equals(this, name);
	}

	public static Name Parse(string fullName)
	{
		if (string.IsNullOrWhiteSpace(fullName))
		{
			return new();
		}

		var tokens = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		if (tokens.Length == 1)
		{
			return new Name(tokens[0], string.Empty);
		}

		var suffix = GetSuffix(tokens);
		var lastNameIndex = tokens.Length - 1 - ((suffix == string.Empty) ? 0 : 1);
		var hasMiddleName = lastNameIndex > 1;

		return new Name(
			tokens.First(),
			tokens[lastNameIndex].TrimEnd(','),
			hasMiddleName ? string.Join(" ", tokens, 1, lastNameIndex - 1) : string.Empty,
			suffix
		);
	}

	private static readonly string[] _suffixList = new[] {
		"senior",
		"junior",
		"sr",
		"jr",
		"snr",
		"jnr",
		"sr.",
		"jr.",
		"snr.",
		"jnr.",
		"ii",
		"iii",
		"iv",
		"v"
	};

	private static string GetSuffix(IEnumerable<string> tokens)
		=> tokens.Count() > 2 && IsSuffix(tokens.Last()) ? tokens.Last() : string.Empty;

	public static bool IsSuffix(string token)
		=> _suffixList.Contains(token, StringComparer.OrdinalIgnoreCase);

	private static string ReplaceIgnoredStrings(string token)
		=> token?.ToUpper() == _ignoredString ? string.Empty : token ?? string.Empty;
}
