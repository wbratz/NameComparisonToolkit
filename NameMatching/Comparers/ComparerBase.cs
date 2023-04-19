namespace NameMatching.Comparers;

public abstract class ComparerBase : IEqualityComparer<Name>
{
	protected static bool CompareRequiredString(string x, string y)
		=> string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	protected static bool CompareRequiredNamePart(IEnumerable<string> x, IEnumerable<string> y)
	{
		if (!x.Any() || !y.Any())
		{
			return false;
		}
		
		var sortedXLastName = x.OrderBy(name => name).ToList();
		var sortedYLastName = y.OrderBy(name => name).ToList();
		return sortedXLastName.SequenceEqual(sortedYLastName, StringComparer.InvariantCultureIgnoreCase);
	}
	

	protected static bool CompareOptionalString(string x, string y)
		=> ReferenceEquals(x, y)
			|| string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)
			|| string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	public abstract bool Equals(Name x, Name y);
	public abstract int GetHashCode(Name obj);
}
