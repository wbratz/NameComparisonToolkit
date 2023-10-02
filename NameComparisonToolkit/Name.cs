using NameComparisonToolkit.Comparers;
using NameComparisonToolkit.Extensions;

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
		=> string.Join(" ", new[]
		{
			string.Join(" ", ToLower(FirstName)),
			string.Join(" ", ToLower(MiddleName)),
			string.Join(" ", ToLower(LastName)),
			includeSuffix ? NormalizeSuffix(Suffix) : string.Empty
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
		FirstName = firstName.IsNullOrWhiteSpace() ? Enumerable.Empty<string>() : ToLower(firstName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
		MiddleName = middleName.IsNullOrWhiteSpace() ? Enumerable.Empty<string>() : ToLower(middleName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
		LastName = lastName.IsNullOrWhiteSpace() ? Enumerable.Empty<string>() : ToLower(lastName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
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
		FirstName = ToLower(firstName);
		MiddleName = ToLower(middleName);
		LastName = ToLower(lastName);
		Suffix = ReplaceIgnoredStrings(NormalizeSuffix(suffix?.Trim() ?? string.Empty));
	}

	/// <summary>
	/// Compares the given name object with the current name object using all available comparison types and returns a list of comparison results.
	/// Uses Equals for each name part given the comparison type
	/// </summary>
	/// <param name="nameToCompare">The name object to compare.</param>
	/// <returns>List of all Comparison results</returns>
	public IEnumerable<ComparisonResult> Matches(Name nameToCompare)
		=> ExecuteComparison(nameToCompare, (comparer, comparisonType, name) => Compare(name, comparer, comparisonType));

	/// <summary>
	/// Compares the given name object with the current name object using all available comparison types and returns a list of comparison results.
	/// Uses Equals for each name part given the comparison type
	/// </summary>
	/// <param name="nameToCompare">The name object to compare.</param>
	/// <returns>List of all Comparison results</returns>
	public IEnumerable<ComparisonResult> MatchesIgnoreOrder(Name nameToCompare)
		=> ExecuteComparison(nameToCompare, (comparer, comparisonType, name) => CompareIgnoreOrder(name, comparer, comparisonType));

	/// <summary>
	/// Checks if the current name object is contained within the given name string using all available comparison types and returns a list of comparison results.
	/// </summary>
	/// <param name="nameToCompare">The name object to compare.</param>
	/// <returns>List of Contains results.</returns>
	public IEnumerable<ComparisonResult> Contains(string nameToCompare)
		=> ExecuteComparison(nameToCompare, (comparer, comparisonType, name) => Contains(name, comparer, comparisonType));

	/// <summary>
	/// Checks if the given name object intersects with the current name object using all available comparison types and returns a list of comparison results.
	/// </summary>
	/// <param name="nameToCompare">The name object to compare.</param>
	/// <returns>List of Intersects results.</returns>
	public IEnumerable<ComparisonResult> Intersects(Name nameToCompare)
		=> ExecuteComparison(nameToCompare, (comparer, comparisonType, name) => Intersects(name, comparer, comparisonType));

	private static IEnumerable<ComparisonResult> ExecuteComparison<T>(T nameToCompare, Func<ComparerBase, ComparisonType, T, ComparisonResult> comparisonFunction)
		=> Enum.GetValues(typeof(ComparisonType))
			.Cast<ComparisonType>()
			.Select(t => comparisonFunction(t.GetComparer(), t, nameToCompare));

	private ComparisonResult Compare(Name name, ComparerBase comparer, ComparisonType comparisonType)
		=> new(comparisonType, comparer.Equals(this, name), comparer.GetSimilarity(this, name));

	private ComparisonResult CompareIgnoreOrder(Name name, ComparerBase comparer, ComparisonType comparisonType)
		=> new(comparisonType, comparer.EqualsIgnoreOrder(this, name), comparer.GetSimilarity(this, name));

	private ComparisonResult Contains(string name, ComparerBase comparer, ComparisonType comparisonType)
		=> new(comparisonType, comparer.Contains(this, name), comparer.GetSimilarity(this, name));

	private ComparisonResult Intersects(Name name, ComparerBase comparer, ComparisonType comparisonType)
		=> new(comparisonType, comparer.Intersects(this, name), comparer.GetSimilarity(this, name));

	private static bool IsSuffix(string token)
		=> _suffixList.Contains(token, StringComparer.OrdinalIgnoreCase);

	private static string ReplaceIgnoredStrings(string token)
		=> token?.ToUpper() == _ignoredString ? string.Empty : token ?? string.Empty;

	private static string NormalizeSuffix(string suffix)
		=> string.IsNullOrEmpty(suffix)
			? string.Empty
			: suffix.Replace(".", "").ToLowerInvariant();

	private static IEnumerable<string> ToLower(IEnumerable<string> namePart)
	{
		namePart = namePart.ToList().ConvertAll(s => s.ToLowerInvariant());
		return namePart;
	}

	public static Name TryParse(string fullName)
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
		var suffixIndex = suffix.Equals(string.Empty) ? tokens.Length : tokens.Length - 1;

		for (var i = 1; i < suffixIndex; i++)
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

	private static readonly string[] _suffixList = {
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
