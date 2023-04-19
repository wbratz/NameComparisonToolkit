namespace NameMatching.Comparers;

public sealed class FirstLast : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> (CompareRequiredNamePart(x.FirstName, y.FirstName)
			&& CompareRequiredNamePart(x.LastName, y.LastName))
			|| CompareTokens(x, y);

	public override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.FirstName))
		&& y.Contains(string.Join(" ", x.LastName));

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.FirstName.Aggregate(0, (hash, firstName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(firstName.ToLowerInvariant()))
				 ^ obj.LastName.Aggregate(0, (hash, lastName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
		};

	private static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			&& !xTokens.Except(yTokens, StringComparer.InvariantCultureIgnoreCase).Any();
	}
}

