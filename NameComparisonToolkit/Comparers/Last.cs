using NameComparisonToolkit.Extensions;
using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Comparers;

internal sealed class Last : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> CompareRequiredNamePart(x.LastName, y.LastName);

	internal override bool EqualsIgnoreOrder(Name x, Name y)
		=> CompareRequiredNamePartIgnoreOrder(x.LastName, y.LastName);

	internal override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.LastName), StringComparison.OrdinalIgnoreCase);

	internal override double GetSimilarity(Name x, Name y)
	=> SimilarityBuilder.Build(x.LastName.Join(" ").ToLowerInvariant(), y.LastName.Join(" ").ToLowerInvariant());

	internal override double GetSimilarity(Name x, string y)
		=> SimilarityBuilder.Build(x.LastName.Join(" ").ToLowerInvariant(), y.ToLowerInvariant());

	internal override bool Intersects(Name x, Name y)
		=> y.LastName.Intersect(x.LastName).Any();

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.LastName.Aggregate(0, (hash, lastName) => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
		};
}
