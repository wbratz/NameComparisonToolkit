namespace NameMatching;

public sealed class Name
{
	private static readonly string _ignoredString = "NONE";
	public string FirstName { get; }
	public IEnumerable<string> LastName { get; }
	public string MiddleName { get; }
	public string Suffix { get; }

	public string GetFullName(bool includeSuffix)
		=> string.Join(" ", new string[]
		{
			FirstName,
			MiddleName,
			string.Join(" ", LastName),
			includeSuffix ? Suffix : string.Empty
		}.Where(x => !string.IsNullOrEmpty(x)));

	public List<string> GetTokenizedName(bool includeSuffix)
		=> GetFullName(includeSuffix).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

	public Name(string firstName, string lastName) : this(firstName, string.Empty, lastName, string.Empty)
	{
	}

	public Name(string firstName, string middleName, string lastName, string suffix)
	{
		FirstName = firstName?.Trim() ?? string.Empty;
		MiddleName = middleName?.Trim() ?? string.Empty;
		LastName = lastName != null ? lastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		Suffix = ReplaceIgnoredStrings(suffix?.Trim() ?? string.Empty);
	}

	public Name(string fullName) : this(Parse(fullName))
	{
	}

	public Name(Name parsedName)
		: this(parsedName.FirstName, parsedName.MiddleName, string.Join(" ", parsedName.LastName), parsedName.Suffix)
	{
	}

	public Name(string firstName, string middleName, IEnumerable<string> lastName, string suffix)
		: this(firstName, middleName, string.Join(" ", lastName), suffix)
	{
	}

	public bool Matches(Name name)
		=> Matches(name, Comparison.ExactMatchIgnoreCase);

	public bool Matches(Name name, ComparisonType comparison)
		=> Matches(name, GetComparer(comparison));

	public bool MatchesAny(IEnumerable<Name> names, ComparisonType comparison)
		=> names.Any(x => Matches(x, GetComparer(comparison)));

	public bool Matches(Name name, IEqualityComparer<Name> comparer)
		=> comparer.Equals(this, name);

	private static IEqualityComparer<Name> GetComparer(ComparisonType comparison)
	{
		return comparison switch
		{
			ComparisonType.ExactMatchIgnoreCase => Comparison.ExactMatchIgnoreCase,
			ComparisonType.FirstNameLastNameIgnoreCase => Comparison.FirstNameLastNameIgnoreCase,
			ComparisonType.LastNameIgnoreCase => Comparison.LastNameIgnoreCase,
			ComparisonType.FirstLastSuffixIgnoreCase => Comparison.FirstLastSuffixIgnoreCase,
			ComparisonType.NoMatch => Comparison.NoMatch,
			_ => Comparison.ExactMatchIgnoreCase,
		};
	}

	private static Name Parse(string fullName)
	{
		if (string.IsNullOrWhiteSpace(fullName))
		{
			throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));
		}

		var tokens = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		if (tokens.Length == 1)
		{
			return new Name(tokens[0], string.Empty);
		}

		var firstName = tokens[0];
		var middleName = string.Empty;
		var lastNames = new List<string>();
		var suffix = tokens.Length > 1 && IsSuffix(tokens[^1]) ? tokens[^1] : string.Empty;

		for (var i = 1; i < tokens.Length; i++)
		{
			if (string.IsNullOrEmpty(middleName))
			{
				middleName = tokens[i];
			}
			else
			{
				lastNames.Add(tokens[i]);
			}
		}

		if (lastNames.Count == 0)
		{
			middleName = string.Empty;
			lastNames.Add(tokens[1]);
		}

		return new Name(firstName, middleName, lastNames, suffix);
	}

	private static bool IsSuffix(string token)
		=> _suffixList.Contains(token, StringComparer.OrdinalIgnoreCase);

	private static string ReplaceIgnoredStrings(string token)
		=> token?.ToUpper() == _ignoredString ? string.Empty : token ?? string.Empty;

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
		"v",
		"vi",
		"vii",
		"viii",
		"ix",
		"x",
	};
}
