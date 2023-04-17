namespace NameMatching.Comparers;

public sealed class FirstNameLastNameComparer : NameComparer
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

		return (CompareRequiredString(x.FirstName, y.FirstName)
			&& CompareRequiredString(x.LastName, y.LastName))
			|| CompareTokens(x, y);
	}

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FirstName)
			^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.LastName)
		};

	protected static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			&& xTokens.All(xt => xt.Equals(yTokens.ElementAt(xTokens.IndexOf(xt)), StringComparison.InvariantCultureIgnoreCase));
	}
}
