namespace NameComparisonToolkit.Tests;
public class ExactMatchTests
{
	[Theory]
	[ClassData(typeof(ExactMatchTestData))]
	public void Equals_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.Matches(name2).ToList();

		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResults[0]);
	}
	
	[Theory]
	[ClassData(typeof(ExactMatchTestData))]
	public void EqualsIgnoreOrder_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.MatchesIgnoreOrder(name2).ToList();

		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResults[1]);
	}
	
	[Theory]
	[ClassData(typeof(ExactMatchTestData))]
	public void Contains_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
	
		var results = name1.Contains(name2.GetFullName()).ToList();
	
		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResults[2]);
	}
	
	[Theory]
	[ClassData(typeof(ExactMatchTestData))]
	public void  Intersects_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
	
		var results = name1.Intersects(name2).ToList();
	
		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResults[3]);
	}
	
}