namespace NameComparisonToolkit.Tests;

public class NameTests
{
	[Fact]
	public void GetTokenizedName_ShouldReturnCorrectTokens()
	{
		var name = new Name("John", "Adam", "Smith", "Jr.");
		var tokensWithSuffix = name.GetTokenizedName(includeSuffix: true);
		var tokensWithoutSuffix = name.GetTokenizedName(includeSuffix: false);

		tokensWithSuffix.Should().Equal(new List<string> { "john", "adam", "smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "john", "adam", "smith" });
		tokensWithSuffix.Should().Equal(new List<string> { "john", "adam", "smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "john", "adam", "smith" });
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMatchingNames_AllMatchTypes()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMixedSuffixPunctuation()
	{
		var name1 = new Name("john", "Adam", "Smith", "Jr");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnFalseForNonMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Doe", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeFalse();
	}


	[Fact]
	public void GetMatchResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.Matches(name2);

		result.Count().Should().Be(typeCount);
	}

	[Fact]
	public void GetContainResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.Contains("name");

		result.Count().Should().Be(typeCount);
	}

	[Fact]
	public void GetIntersectResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John Adam", "Joel", "Smith", "Jr.");

		var result = name1.Intersects(name2);

		result.Count().Should().Be(typeCount);
	}
}