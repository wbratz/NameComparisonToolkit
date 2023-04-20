using NameComparisonToolkit.Confidence;
using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Comparers;

public sealed class ExactMatch : ComparerBase
{
	public override bool Equals(Name x, Name y)
	{
		return CompareRequiredNamePart(x.FirstName, y.FirstName)
			   && CompareRequiredNamePart(x.MiddleName, y.MiddleName)
			   && CompareRequiredNamePart(x.LastName, y.LastName)
			   && CompareOptionalString(x.Suffix, y.Suffix)
			   || CompareTokens(x, y);
	}

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

	public override double GetConfidence(Name x, Name y)
		=> ConfidenceBuilder.Build(x.FirstName.Join(" "), y.FirstName.Join(" "))
			* ConfidenceBuilder.Build(x.MiddleName.Join(" "), y.MiddleName.Join(" "))
			* ConfidenceBuilder.Build(x.LastName.Join(" "), y.LastName.Join(" "))
			* ConfidenceBuilder.Build(x.Suffix, y.Suffix);

	private static bool CompareTokens(Name x, Name y)
	{
		var xTokens = x.GetTokenizedName(true);
		var yTokens = y.GetTokenizedName(true);

		return xTokens.Count == yTokens.Count
			&& !xTokens.Except(yTokens, StringComparer.InvariantCultureIgnoreCase).Any();
	}

	public override bool Contains(Name x, string y)
		=> y.Contains(string.Join(" ", x.FirstName))
		&& y.Contains(string.Join(" ", x.MiddleName))
		&& y.Contains(string.Join(" ", x.LastName))
		&& y.Contains(string.Join(" ", x.Suffix));

	public override bool Contains(Name x, Name y) 
		=> y.FirstName.Intersect(x.FirstName).Any()
			&& y.MiddleName.Intersect(x.MiddleName).Any()
			&& y.LastName.Intersect(x.LastName).Any()
			&& CompareRequiredString(x.Suffix, y.Suffix);
}
