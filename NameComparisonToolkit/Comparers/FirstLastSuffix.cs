using NameComparisonToolkit.Confidence;
using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Comparers;

public sealed class FirstLastSuffix : ComparerBase
{
	public override bool Equals(Name x, Name y)
		=> (CompareRequiredNamePart(x.FirstName, y.FirstName)
				&& CompareRequiredNamePart(x.LastName, y.LastName)
				&& CompareRequiredString(x.Suffix, y.Suffix))
			   || CompareTokens(x, y);

	public override bool EqualsIgnoreOrder(Name x, Name y)
		=> CompareRequiredNamePartIgnoreOrder(x.FirstName, y.FirstName)
			   && CompareRequiredNamePartIgnoreOrder(x.LastName, y.LastName)
			   && CompareOptionalString(x.Suffix, y.Suffix)
			   || CompareTokens(x, y);

	public override bool Contains(Name x, string y)
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

	public override double GetConfidence(Name x, Name y)
		=> ConfidenceBuilder.Build(x.FirstName.Join(" "), y.FirstName.Join(" "))
			* ConfidenceBuilder.Build(x.LastName.Join(" "), y.LastName.Join(" "))
			* ConfidenceBuilder.Build(x.Suffix, y.Suffix);

	public override bool Intersects(Name x, Name y)
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
