namespace NameMatching.Comparers;

public sealed class LastNameComparer : NameComparer
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

		return CompareRequiredString(x.LastName, y.LastName);
	}

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.LastName)
		};
}
