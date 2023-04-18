namespace NameMatching.Comparers;

public sealed class FirstLast : ComparerBase
{
	public override bool Equals(Name x, Name y)
	{
		if (x == null || y == null)
		{
			return false;
		}

		if (x == y)
		{
			return true;
		}

		var sortedXLastName = x.LastName.OrderBy(name => name).ToList();
		var sortedYLastName = y.LastName.OrderBy(name => name).ToList();

		return (CompareRequiredString(x.FirstName, y.FirstName)
			&& sortedXLastName.SequenceEqual(sortedYLastName, StringComparer.InvariantCultureIgnoreCase))
			|| CompareTokens(x, y);
	}

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FirstName.ToLowerInvariant())
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

