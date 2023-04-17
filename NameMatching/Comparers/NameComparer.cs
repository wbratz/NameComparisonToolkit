namespace NameMatching.Comparers;

public abstract class NameComparer : IEqualityComparer<Name>
{
	protected static bool CompareRequiredString(string x, string y)
		=> string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	protected static bool CompareOptionalString(string x, string y)
		=> ReferenceEquals(x, y)
			|| string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)
			|| string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

	public abstract bool Equals(Name x, Name y);
	public abstract int GetHashCode(Name obj);
}
