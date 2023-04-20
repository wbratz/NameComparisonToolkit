using NameComparisonToolkit.Comparers;

namespace NameComparisonToolkit;

public sealed class Name
{
	private static readonly string _ignoredString = "NONE";

	/// <summary>
	/// A collection of first name strings.
	/// </summary>
	public IEnumerable<string> FirstName { get; }

	/// <summary>
	/// A collection of last name strings.
	/// </summary>
	public IEnumerable<string> LastName { get; }

	/// <summary>
	/// A collection of middle name strings.
	/// </summary>
	public IEnumerable<string> MiddleName { get; }

	/// <summary>
	/// A string representing the name suffix.
	/// </summary>
	public string Suffix { get; }

	/// <summary>
	/// Generates a full name string by concatenating the first name, middle name, last name, and suffix (if specified).
	/// </summary>
	/// <param name="includeSuffix">A boolean value indicating whether to include the suffix in the full name. Default is true.</param>
	/// <returns>A string representing the full name.</returns>
	public string GetFullName(bool includeSuffix = true)
		=> string.Join(" ", new string[]
		{
		string.Join(" ", FirstName),
		string.Join(" ", MiddleName),
		string.Join(" ", LastName),
		includeSuffix ? Suffix : string.Empty
		}.Where(x => !string.IsNullOrEmpty(x)));


	/// <summary>
	/// Retrieves a tokenized version of the full name as a list of strings.
	/// </summary>
	/// <param name="includeSuffix">A boolean value indicating whether to include the suffix in the tokenized name. Default is true.</param>
	/// <returns>A list of strings representing the tokenized full name.</returns>
	public List<string> GetTokenizedName(bool includeSuffix = true)
		=> GetFullName(includeSuffix).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

	/// <summary>
	/// Creates a new instance of the Name class with the provided first name and last name.
	/// </summary>
	/// <param name="firstName">A string representing the first name.</param>
	/// <param name="lastName">A string representing the last name.</param>
	public Name(string firstName, string lastName) : this(firstName, string.Empty, lastName, string.Empty)
	{
	}

	/// <summary>
	/// Creates a new instance of the Name class with the provided first name, middle name, last name, and suffix.
	/// </summary>
	/// <param name="firstName">A string representing the first name.</param>
	/// <param name="middleName">A string representing the middle name.</param>
	/// <param name="lastName">A string representing the last name.</param>
	/// <param name="suffix">A string representing the name suffix.</param>
	public Name(string firstName, string middleName, string lastName, string suffix)
	{
		FirstName = firstName != null ? firstName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		MiddleName = middleName != null ? middleName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		LastName = lastName != null ? lastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();
		Suffix = ReplaceIgnoredStrings(NormalizeSuffix(suffix?.Trim() ?? string.Empty));
	}

	/// <summary>
	/// Creates a new instance of the Name class with the provided first name, middle name, last name, and suffix collections.
	/// </summary>
	/// <param name="firstName">A collection of first name strings.</param>
	/// <param name="middleName">A collection of middle name strings.</param>
	/// <param name="lastName">A collection of last name strings.</param>
	/// <param name="suffix">A string representing the name suffix.</param>
	public Name(IEnumerable<string> firstName, IEnumerable<string> middleName, IEnumerable<string> lastName, string suffix)
	{
		FirstName = firstName;
		MiddleName = middleName;
		LastName = lastName;
		Suffix = ReplaceIgnoredStrings(NormalizeSuffix(suffix?.Trim() ?? string.Empty));
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="nameToCompare">The name object to compare.</param>
	/// <returns>List of all Match results</returns>
	public IEnumerable<ComparisonResult> Matches(Name nameToCompare) //TODO:potentially Rename to RunMatches GetMatches
	{
		return (from ComparisonType t in Enum.GetValues(typeof(ComparisonType)) select Compare(nameToCompare, t.GetComparer())).ToList();
	}
	
	/// <summary>
	/// Determines if the given name object matches the current name object using the ExactMatchIgnoreCase comparer.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <returns>True if the names match, otherwise false.</returns>
	public ComparisonResult Compare(Name name)
		=> Compare(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	/// <summary>
	/// Determines if the given name object matches the current name object using the specified comparison type.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <param name="comparison">The comparison type to use.</param>
	/// <returns>True if the names match, otherwise false.</returns>
	public ComparisonResult Compare(Name name, ComparisonType comparison)
		=> Compare(name, comparison.GetComparer());

	/// <summary>
	/// Determines if the given name object matches the current name object using the specified comparer.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <param name="comparer">The comparer to use.</param>
	/// <returns>True if the names match, otherwise false.</returns>
	public ComparisonResult Compare(Name name, IEqualityComparer<Name> comparer)
		=> new()
		{
			Method = comparer.GetType().Name,
			IsMatch = comparer.Equals(this, name),
			Confidence = 0
		};

	/// <summary>
	/// Determines if the current name object matches any of the names in the given enumerable using the specified comparison type.
	/// </summary>
	/// <param name="names">An enumerable of name objects.</param>
	/// <param name="comparison">The comparison type to use.</param>
	/// <returns>True if the current name object matches any name in the enumerable, otherwise false.</returns>
	public bool MatchesAny(IEnumerable<Name> names, ComparisonType comparison)
		=> names.Any(x => Compare(x, comparison.GetComparer()).IsMatch);

	/// <summary>
	/// Determines if the given name object matches the current name object, ignoring the order of the name parts, using the ExactMatchIgnoreCase comparer.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <returns>True if the name objects match when ignoring the order of the name parts, otherwise false.</returns>
	public bool MatchesIgnoreOrder(Name name)
		=> MatchesIgnoreOrder(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	/// <summary>
	/// Determines if the given name object matches the current name object, ignoring the order of the name parts, using the specified comparison type.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <param name="comparison">The comparison type to use.</param>
	/// <returns>True if the name objects match when ignoring the order of the name parts, otherwise false.</returns>
	public bool MatchesIgnoreOrder(Name name, ComparisonType comparison)
		=> MatchesIgnoreOrder(name, comparison.GetComparer());

	/// <summary>
	/// Determines if the given name string is contained within the current name object using the specified comparison type.
	/// </summary>
	/// <param name="name">The name string to compare.</param>
	/// <param name="comparison">The comparison type to use.</param>
	/// <returns>True if the name string is contained within the current name object, otherwise false.</returns>
	public bool Contains(string name, ComparisonType comparison)
		=> Contains(name, comparison.GetComparer());

	/// <summary>
	/// Determines if the given name string is contained within the current name object using the ExactMatchIgnoreCase comparer.
	/// All parts of the name object must be contained in the string.
	/// </summary>
	/// <param name="name">The name string to compare.</param>
	/// <returns>True if the name string is contained within the current name object, otherwise false.</returns>
	public bool Contains(string name)
		=> Contains(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	/// <summary>
	/// Determines if the given name object intersects with the current name object using the ExactMatchIgnoreCase comparer.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <returns>True if the name object intersects with the current name object, otherwise false.</returns>
	public bool Intersects(Name name)
		=> Intersects(name, ComparisonType.ExactMatchIgnoreCase.GetComparer());

	/// <summary>
	/// Determines if the given name object intersects with the current name object using the specified comparison type.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <param name="comparison">The comparison type to use.</param>
	/// <returns>True if the name object intersects with the current name object, otherwise false.</returns>
	public bool Intersects(Name name, ComparisonType comparison)
		=> Intersects(name, comparison.GetComparer());

	/// <summary>
	/// Calculates the confidence score for the similarity between the current name object and the given name object using the specified comparer.
	/// </summary>
	/// <param name="name">The name object to compare.</param>
	/// <param name="comparison">The comparer to use.</param>
	/// <returns>A double value representing the confidence score.</returns>
	public double GetConfidence(Name name, ComparisonType comparison)
		=> GetConfidence(name, comparison.GetComparer());

	private bool MatchesIgnoreOrder(Name name, ComparerBase comparer)
		=> comparer.EqualsIgnoreOrder(this, name);

	private double GetConfidence(Name name, ComparerBase comparer)
		=> comparer.GetConfidence(this, name);

	private bool Contains(string name, ComparerBase comparer)
		=> comparer.Contains(this, name);

	private bool Intersects(Name name, ComparerBase comparer)
		=> comparer.Intersects(this, name);

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
