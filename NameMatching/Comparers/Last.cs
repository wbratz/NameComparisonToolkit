namespace NameMatching.Comparers;

public sealed class Last : ComparerBase
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

		return sortedXLastName.SequenceEqual(sortedYLastName, StringComparer.InvariantCultureIgnoreCase);
	}

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.LastName.Aggregate(0, (hash, lastName) => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
		};
}
