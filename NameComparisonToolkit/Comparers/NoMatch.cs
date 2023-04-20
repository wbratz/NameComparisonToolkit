using NameComparisonToolkit.Confidence;

namespace NameComparisonToolkit.Comparers;

public sealed class NoMatch : ComparerBase
{
	public override bool Contains(Name x, string y) => true;
	public override bool Equals(Name x, Name? y) => true;
	public override double GetConfidence(Name x, Name y) => 1.0;
	public override int GetHashCode(Name obj) => 0;
}
