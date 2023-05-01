namespace NameComparisonToolkit.Comparers;

internal abstract class ComparerBase : IEqualityComparer<Name>
{
	internal static bool CompareRequiredString(string x, string y)
		=> string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	internal static bool CompareRequiredNamePart(IEnumerable<string> x, IEnumerable<string> y)
	{
		if (!x.Any() || !y.Any())
		{
			return false;
		}

		return x.SequenceEqual(y, StringComparer.InvariantCultureIgnoreCase);
	}

	internal static bool CompareRequiredNamePartIgnoreOrder(IEnumerable<string> x, IEnumerable<string> y)
	{
		if (!x.Any() || !y.Any())
		{
			return false;
		}

		var sortedXName = x.OrderBy(name => name).ToList();
		var sortedYName = y.OrderBy(name => name).ToList();
		return sortedXName.SequenceEqual(sortedYName, StringComparer.InvariantCultureIgnoreCase);
	}

	internal static bool CompareOptionalNamePartIgnoreOrder(IEnumerable<string> x, IEnumerable<string> y)
	{
		var sortedXName = x.OrderBy(name => name).ToList();
		var sortedYName = y.OrderBy(name => name).ToList();
		return (!x.Any() && !y.Any())
			|| (x.Any() && y.Any() && sortedXName.SequenceEqual(sortedYName, StringComparer.InvariantCultureIgnoreCase));
	}

	internal static bool CompareOptionalNamePart(IEnumerable<string> x, IEnumerable<string> y)
		=> (!x.Any() && !y.Any()) || (x.Any() && y.Any() && x.SequenceEqual(y, StringComparer.OrdinalIgnoreCase));

	internal static bool CompareOptionalString(string x, string y)
		=> ReferenceEquals(x, y)
			|| string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)
			|| string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	public abstract bool Equals(Name x, Name y);
	internal abstract bool EqualsIgnoreOrder(Name x, Name y);
	internal abstract bool Contains(Name x, string y);
	internal abstract bool Intersects(Name x, Name y);
	internal abstract double GetSimilarity(Name x, Name y);
	internal abstract double GetSimilarity(Name x, string y);
	public abstract int GetHashCode(Name obj);
}
