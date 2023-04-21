using NameComparisonToolkit.Extensions;
using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Comparers;

internal sealed class ExactMatch : ComparerBase
{
	public override bool Equals(Name x, Name y)
	{
		return CompareRequiredNamePart(x.FirstName, y.FirstName)
			   && CompareRequiredNamePart(x.MiddleName, y.MiddleName)
			   && CompareRequiredNamePart(x.LastName, y.LastName)
			   && CompareOptionalString(x.Suffix, y.Suffix)
			   || CompareTokens(x, y);
	}

	internal override bool EqualsIgnoreOrder(Name x, Name y)
		=> CompareRequiredNamePartIgnoreOrder(x.FirstName, y.FirstName)
			   && CompareRequiredNamePartIgnoreOrder(x.MiddleName, y.MiddleName)
			   && CompareRequiredNamePartIgnoreOrder(x.LastName, y.LastName)
			   && CompareOptionalString(x.Suffix, y.Suffix)
			   || CompareTokens(x, y);

	public override int GetHashCode(Name obj)
	{
		return obj switch
		{
			null => 0,
			_ => obj.FirstName.Aggregate(0, (hash, firstName)
					  => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(firstName.ToLowerInvariant()))
				  ^ obj.MiddleName.Aggregate(0, (hash, middleName)
						=> hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(middleName.ToLowerInvariant()))
				  ^ obj.LastName.Aggregate(0, (hash, lastName)
						=> hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
				 ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Suffix.ToLowerInvariant())
		};
	}

	internal override double GetSimilarity(Name x, Name y)
		=> SimilarityBuilder.Build(string.Join(" ", x.FirstName).ToLowerInvariant(), string.Join(" ", y.FirstName).ToLowerInvariant())
			* SimilarityBuilder.Build(string.Join(" ", x.MiddleName).ToLowerInvariant(), string.Join(" ", y.MiddleName).ToLowerInvariant())
			* SimilarityBuilder.Build(string.Join(" ", x.LastName).ToLowerInvariant(), string.Join(" ", y.LastName).ToLowerInvariant())
			* SimilarityBuilder.Build(x.Suffix.ToLowerInvariant(), y.Suffix.ToLowerInvariant());

	internal override double GetSimilarity(Name x, string y)
		=> SimilarityBuilder.Build(x.GetFullName().ToLowerInvariant(), y.ToLowerInvariant());

	private static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			&& !xTokens.Except(yTokens, StringComparer.InvariantCultureIgnoreCase).Any();
	}

	internal override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.FirstName), StringComparison.OrdinalIgnoreCase)
			&& y.Contains(string.Join(" ", x.MiddleName), StringComparison.OrdinalIgnoreCase)
			&& y.Contains(string.Join(" ", x.LastName), StringComparison.OrdinalIgnoreCase)
			&& y.Contains(string.Join(" ", x.Suffix), StringComparison.OrdinalIgnoreCase);

	internal override bool Intersects(Name x, Name y)
		=> y.FirstName.Intersect(x.FirstName).Any()
			&& ((!y.MiddleName.Any() && !x.MiddleName.Any()) || y.MiddleName.Intersect(x.MiddleName).Any())
			&& y.LastName.Intersect(x.LastName).Any()
			&& CompareRequiredString(x.Suffix, y.Suffix);
}
