using NameComparisonToolkit.Confidence;
using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Comparers;

public sealed class Last : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> CompareRequiredNamePart(x.LastName, y.LastName);

	public override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.LastName));

	public override double GetConfidence(Name x, Name y)
		=> ConfidenceBuilder.Build(x.LastName.Join(" "), y.LastName.Join(" "));

	public override bool Contains(Name x, Name y)
		=> y.LastName.Intersect(x.LastName).Any();

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.LastName.Aggregate(0, (hash, lastName) => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
		};
}
