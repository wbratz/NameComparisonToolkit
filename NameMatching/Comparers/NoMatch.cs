namespace NameMatching.Comparers;

public sealed class NoMatch : ComparerBase
{
	public override bool Contains(Name x, string y) => true;
	public override bool Equals(Name x, Name? y) => true;
	public override int GetHashCode(Name obj) => 0;
}
