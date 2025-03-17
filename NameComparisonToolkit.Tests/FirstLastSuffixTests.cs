using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Tests;
public class FirstLastSuffixTests
{
	[Theory]
	[ClassData(typeof(FirstLastSuffixTestData))]
	public void Equals_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, object[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.Matches(name2).ToList();

		var results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLastSuffix));
		results.FirstOrDefault()?.IsMatch.Should().Be((bool)expectedResults[0]);
	}

	[Theory]
	[ClassData(typeof(FirstLastSuffixTestData))]
	public void EqualsIgnoreOrder_ShouldCompareCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, object[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.MatchesIgnoreOrder(name2).ToList();

		var results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLastSuffix));
		results.FirstOrDefault()?.IsMatch.Should().Be((bool)expectedResults[1]);
	}

	[Theory]
	[ClassData(typeof(FirstLastSuffixTestData))]
	public void Contains_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, object[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.Contains(name2.GetFullName()).ToList();

		var results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLastSuffix));
		results.FirstOrDefault()?.IsMatch.Should().Be((bool)expectedResults[2]);
	}

	[Theory]
	[ClassData(typeof(FirstLastSuffixTestData))]
	public void Intersects_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, object[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.Intersects(name2).ToList();

		var results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLastSuffix));
		results.FirstOrDefault()?.IsMatch.Should().Be((bool)expectedResults[3]);
	}

	[Theory]
	[ClassData(typeof(FirstLastSuffixTestData))]
	public void Similarity_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, object[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
		var options = new SimilarityOptions { Algorithm = SimilarityAlgorithmType.DamerauLevenshtein };

		var allResults = name1.Matches(name2, options).ToList();

		var results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLastSuffix));
		var res = expectedResults[3];

		results.FirstOrDefault()?.Similarity.Should().BeLessThanOrEqualTo((double)expectedResults[4]);
	}
}