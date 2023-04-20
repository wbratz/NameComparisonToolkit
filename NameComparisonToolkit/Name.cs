using NameComparisonToolkit.Comparers;

namespace NameComparisonToolkit;

public sealed class Name
{
	private static readonly string _ignoredString = "NONE";
	public IEnumerable<string> FirstName { get; }
	public IEnumerable<string> LastName { get; }
	public IEnumerable<string> MiddleName { get; }
	public string Suffix { get; }

	public string GetFullName(bool includeSuffix)
		=> string.Join(" ", new string[]
		{
			string.Join(" ", FirstName),
			string.Join(" ", MiddleName),
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
		FirstName = firstName != null ? firstName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		MiddleName = middleName != null ? middleName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		LastName = lastName != null ? lastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		Suffix = ReplaceIgnoredStrings(NormalizeSuffix(suffix?.Trim() ?? string.Empty));
	}
	public Name(IEnumerable<string> firstName, IEnumerable<string> middleName, IEnumerable<string> lastName, string suffix)
	{
		FirstName = firstName ?? Enumerable.Empty<string>();
		MiddleName = middleName ?? Enumerable.Empty<string>();
		LastName = lastName ?? Enumerable.Empty<string>();
		Suffix = ReplaceIgnoredStrings(NormalizeSuffix(suffix?.Trim() ?? string.Empty));
	}

	public bool Matches(Name name)
		=> Matches(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	/// <summary>
	/// Matches a name string to a name object parts using desired comparison
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public bool Contains(string name, ComparisonType comparison)
		=> Contains(name, comparison.GetComparer());

	/// <summary>
	/// Matches a name string to a name object parts using exact match rules
	/// All parts of the name object must be contained in the string
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public bool Contains(string name)
		=> Contains(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	public bool Contains(Name name)
		=> Contains(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	public bool Contains(Name name, ComparisonType comparison)
		=> Contains(name, comparison.GetComparer());

	public bool Matches(Name name, ComparisonType comparison)
		=> Matches(name, comparison.GetComparer());

	public bool MatchesAny(IEnumerable<Name> names, ComparisonType comparison)
		=> names.Any(x => Matches(x, comparison.GetComparer()));

	public bool Matches(Name name, IEqualityComparer<Name> comparer)
		=> comparer.Equals(this, name);

	public double GetConfidence(Name name, ComparisonType comparison)
		=> GetConfidence(name, comparison.GetComparer());

	private double GetConfidence(Name name, ComparerBase comparer)
		=> comparer.GetConfidence(this, name);

	private bool Contains(string name, ComparerBase comparer)
		=> comparer.Contains(this, name);

	private bool Contains(Name name, ComparerBase comparer)
		=> comparer.Contains(this, name);

	// kept for future development
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

		return new Name(firstName, middleName, string.Join(" ", lastNames), suffix);
	}

	private static bool IsSuffix(string token)
		=> _suffixList.Contains(token, StringComparer.OrdinalIgnoreCase);

	private static string ReplaceIgnoredStrings(string token)
		=> token?.ToUpper() == _ignoredString ? string.Empty : token ?? string.Empty;

	private static string NormalizeSuffix(string suffix)
		=> string.IsNullOrEmpty(suffix)
			? string.Empty
			: suffix.Replace(".", "").ToLowerInvariant();

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
