﻿using NameComparisonToolkit.Extensions;
using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Comparers;

internal sealed class FirstLastSuffix : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> (CompareRequiredNamePart(x.FirstName, y.FirstName)
				&& CompareRequiredNamePart(x.LastName, y.LastName)
				&& CompareRequiredString(x.Suffix, y.Suffix));

	internal override bool EqualsIgnoreOrder(Name x, Name y)
		=> CompareRequiredNamePartIgnoreOrder(x.FirstName, y.FirstName)
			   && CompareRequiredNamePartIgnoreOrder(x.LastName, y.LastName)
			   && CompareOptionalString(x.Suffix, y.Suffix)
			   || CompareTokens(x, y);

	internal override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.FirstName), StringComparison.OrdinalIgnoreCase)
			&& y.Contains(string.Join(" ", x.LastName), StringComparison.OrdinalIgnoreCase)
			&& y.Contains(string.Join(" ", x.Suffix), StringComparison.OrdinalIgnoreCase);

	public override int GetHashCode(Name obj)
		=> obj switch
		{
			null => 0,
			_ => obj.FirstName.Aggregate(0, (hash, firstName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(firstName.ToLowerInvariant()))
				 ^ obj.LastName.Aggregate(0, (hash, lastName)
					 => hash ^ StringComparer.OrdinalIgnoreCase.GetHashCode(lastName.ToLowerInvariant()))
				 ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Suffix.ToLowerInvariant())
		};

	internal override double GetSimilarity(Name x, Name y)
		=> SimilarityBuilder.Build(x.FirstName.Join(" ").ToLowerInvariant(), y.FirstName.Join(" ").ToLowerInvariant())
			* SimilarityBuilder.Build(x.LastName.Join(" ").ToLowerInvariant(), y.LastName.Join(" ").ToLowerInvariant())
			* SimilarityBuilder.Build(x.Suffix.ToLowerInvariant(), y.Suffix.ToLowerInvariant());

	internal override double GetSimilarity(Name x, string y)
		=> SimilarityBuilder.Build($"{x.FirstName.Join(" ")} {x.LastName.Join(" ")} {x.Suffix}".ToLowerInvariant(), y.ToLowerInvariant());


	internal override bool Intersects(Name x, Name y)
		=> y.FirstName.Intersect(x.FirstName).Any()
			&& y.LastName.Intersect(x.LastName).Any()
			&& CompareRequiredString(x.Suffix, y.Suffix);

	private static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			  && !xTokens.Except(yTokens, StringComparer.InvariantCultureIgnoreCase).Any();
	}
}
