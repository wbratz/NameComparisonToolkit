using System.Text.RegularExpressions;

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
	public void Matches_ShouldReturnTrueForMatchingNames_AllMatchTypes()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var results = name1.Matches(name2).ToList();
	
		foreach (var result in results)
		{
			if (result.Method.Equals("ExactMatch"))
			{
				result.IsMatch.Should().Be(true);
			}
		}
		
		//name1.Compare(name2).IsMatch.Should().BeTrue();
	}

	// [Fact]
	// public void Matches_ShouldReturnTrueForMixedSuffixPunctuation()
	// {
	// 	var name1 = new Name("John", "Adam", "Smith", "Jr");
	// 	var name2 = new Name("John", "Adam", "Smith", "Jr.");
	//
	// 	name1.Compare(name2).IsMatch.Should().BeTrue();
	// }
	//
	// [Fact]
	// public void Matches_ShouldReturnFalseForNonMatchingNames()
	// {
	// 	var name1 = new Name("John", "Adam", "Smith", "Jr.");
	// 	var name2 = new Name("John", "Adam", "Doe", "Jr.");
	//
	// 	name1.Compare(name2).IsMatch.Should().BeFalse();
	// }
	//
	//
	// [Fact]
	// public void GetMatchResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	// {
	// 	var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
	// 	var name1 = new Name("John", "Adam", "Smith", "Jr.");
	// 	var name2 = new Name("John", "Adam", "Smith", "Jr.");
	//
	// 	var result = name1.Matches(name2);
	//
	// 	result.Count().Should().Be(typeCount);
	// }
	//
	// [Fact]
	// public void GetContainResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	// {
	// 	var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
	// 	var name1 = new Name("John", "Adam", "Smith", "Jr.");
	// 	var name2 = new Name("John", "Adam", "Smith", "Jr.");
	//
	// 	var result = name1.Contains(name2);
	//
	// 	result.Count().Should().Be(typeCount);
	// }
	//
	// [Fact]
	// public void GetIntersectResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	// {
	// 	var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
	// 	var name1 = new Name("John", "Adam", "Smith", "Jr.");
	// 	var name2 = new Name("John Adam", "Joel", "Smith", "Jr.");
	//
	// 	var result = name1.Intersects(name2);
	//
	// 	result.Count().Should().Be(typeCount);
	// }
}