using NameComparisonToolkit.Extensions;
using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Comparers;

internal sealed class FirstInitialLast : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> CompareRequiredNamePart(x.FirstName.Select(fn => fn[..1]), y.FirstName.Select(fn => fn[..1]))
		   && CompareRequiredNamePart(x.LastName, y.LastName);

	internal override bool EqualsIgnoreOrder(Name x, Name y)
		=> CompareRequiredNamePartInitialIgnoreOrder(x.FirstName.Select(fn => fn[..1]), y.FirstName.Select(fn => fn[..1]))
		   && CompareRequiredNamePartIgnoreOrder(x.LastName, y.LastName)
		   || CompareTokens(x, y);

	internal override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.FirstName.Select(fn => fn[..1])), StringComparison.OrdinalIgnoreCase)
		   && y.Contains(string.Join(" ", x.LastName), StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.FirstName.Select(fn => fn[..1]).Aggregate(0, (hash, firstName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(firstName.ToLowerInvariant()))
				 ^ obj.LastName.Aggregate(0, (hash, lastName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
		};

	internal override double GetSimilarity(Name x, Name y)
		=> SimilarityBuilder.Build(x.FirstName.Select(fn => fn[..1]).Join(" ").ToLowerInvariant(), y.FirstName.Select(fn => fn[..1]).Join(" ").ToLowerInvariant())
		   * SimilarityBuilder.Build(x.LastName.Join(" ").ToLowerInvariant(), y.LastName.Join(" ").ToLowerInvariant());

	internal override double GetSimilarity(Name x, string y)
		=> SimilarityBuilder.Build($"{x.FirstName.Select(fn => fn[..1]).Join(" ")} {x.LastName.Join(" ")}".ToLowerInvariant(), y.ToLowerInvariant());

	internal override bool Intersects(Name x, Name y)
		=> y.FirstName.Select(fn => fn[..1]).Intersect(x.FirstName.Select(fn => fn[..1])).Any()
		   && y.LastName.Intersect(x.LastName).Any();

	private static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			&& !xTokens.Except(yTokens, StringComparer.InvariantCultureIgnoreCase).Any();
	}
}