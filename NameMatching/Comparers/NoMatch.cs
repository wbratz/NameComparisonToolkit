namespace NameMatching.Comparers;

public sealed class NoMatch : ComparerBase
{
	public override bool Equals(Name x, Name? y) => true;
	public override int GetHashCode(Name obj) => 0;
}
