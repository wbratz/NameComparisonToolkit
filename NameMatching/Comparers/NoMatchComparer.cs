namespace NameMatching.Comparers;

public sealed class NoMatchComparer : IEqualityComparer<Name>
{
	public bool Equals(Name x, Name? y) => true;

	public int GetHashCode(Name obj) => 0;
}
