namespace NameComparisonToolkit.Tests;

public class NameTests
{
	[Fact]
	public void GetTokenizedName_ShouldReturnCorrectTokens()
	{
		var name = new Name("John", "Adam", "Smith", "Jr.");
		var tokensWithSuffix = name.GetTokenizedName(includeSuffix: true);
		var tokensWithoutSuffix = name.GetTokenizedName(includeSuffix: false);

		tokensWithSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith" });
		tokensWithSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith" });
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		name1.Compare(name2).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMixedSuffixPunctuation()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		name1.Compare(name2).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnFalseForNonMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Doe", "Jr.");

		name1.Compare(name2).IsMatch.Should().BeFalse();
	}

	[Fact]
	public void MatchesAny_ShouldReturnTrueForMatchingNamesInList()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var names = new List<Name>
		{
			new Name("Jane", "Adam", "Smith", "Jr."),
			new Name("John", "Adam", "Smith", "Jr.")
		};

		name1.MatchesAny(names, ComparisonType.ExactMatchIgnoreCase).Should().BeTrue();
	}

	[Fact]
	public void MatchesAny_ShouldReturnFalseForNonMatchingNamesInList()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var names = new List<Name>
		{
			new Name("Jane", "Adam", "Smith", "Jr."),
			new Name("John", "Adam", "Doe", "Jr.")
		};

		name1.MatchesAny(names, ComparisonType.ExactMatchIgnoreCase).Should().BeFalse();
	}

	[Fact]
	public void GetMatchResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.GetMatchResults(name2);

		result.Count().Should().Be(typeCount);
	}
	
	[Fact]
	public void GetContainResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.GetContainResults(name2);

		result.Count().Should().Be(typeCount);
	}
	
	[Fact]
	public void GetIntersectResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John Adam", "Joel", "Smith", "Jr.");

		var result = name1.GetIntersectResults(name2);

		result.Count().Should().Be(typeCount);
	}
}