namespace NameMatching.Tests;

public class NameTests
{
	[Fact]
	public void Constructor_FullName_ShouldParseCorrectly()
	{
		var name = new Name("John Adam Smith Jr.");

		name.FirstName.Should().Be("John");
		name.MiddleName.Should().Be("Adam");
		name.LastName.First().Should().Be("Smith");
		name.Suffix.Should().Be("Jr.");
	}

	[Fact]
	public void GetFullName_ShouldReturnCorrectFullName()
	{
		var name = new Name("John", "Adam", "Smith", "Jr.");
		var fullNameWithSuffix = name.GetFullName(includeSuffix: true);
		var fullNameWithoutSuffix = name.GetFullName(includeSuffix: false);

		fullNameWithSuffix.Should().Be("John Adam Smith Jr.");
		fullNameWithoutSuffix.Should().Be("John Adam Smith");
	}

	[Fact]
	public void GetTokenizedName_ShouldReturnCorrectTokens()
	{
		var name = new Name("John", "Adam", "Smith", "Jr.");
		var tokensWithSuffix = name.GetTokenizedName(includeSuffix: true);
		var tokensWithoutSuffix = name.GetTokenizedName(includeSuffix: false);

		tokensWithSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith", "Jr." });
		tokensWithoutSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith" });
		tokensWithSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith", "Jr." });
		tokensWithoutSuffix.Should().Equal(new List<string> { "John", "Adam", "Smith" });
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		name1.Matches(name2).Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnFalseForNonMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Doe", "Jr.");

		name1.Matches(name2).Should().BeFalse();
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
}