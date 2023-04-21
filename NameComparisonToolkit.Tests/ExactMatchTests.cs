namespace NameComparisonToolkit.Tests;
public class ExactMatchTests
{
	[Theory]
	[InlineData("John", "Adam", "Smith", "jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "Jane", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Eve", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Doe", "Jr.", false)]
	[InlineData("John", "", "Smith Jones", "", "John", "", "Smith Jones", "", true)]
	[InlineData("John", "", "Smith Jones", "", "John", "", "Jones Smith", "", true)]
	[InlineData("John", "", "Smith", "", "John", "", "Smith Jones", "", false)]
	public void Equals_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.Matches(name2).ToList();

		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John James", "Adam Michael", "Smith Doe", "Jr.", "James John", "Michael Adam", "Doe Smith", "Jr.", true)]
	[InlineData("John James", "Adam Michael", "Smith Doe", "Sr.", "James John", "Michael Adam", "Smith Doe", "Sr.", true)]
	[InlineData("John James", "Adam Michael", "Smith Doe", "Jr.", "James John", "Michael Adam", "Smith", "Jr.", false)]
	[InlineData("John James", "Adam Michael", "Smith Doe", "Sr.", "James John", "Michael Adam", "Smith Doe", "", false)]
	public void EqualsIgnoreOrder_ShouldCompareCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.MatchesIgnoreOrder(name2).ToList();

		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "James", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam James", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("William", "", "Clayton", "", "William", "", "Clayton", "", true)]
	public void Intersects_ShouldCompareExactMatchCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.Intersects(name2).ToList();

		var exactMatchResult = results.Where(x => x.ComparisonType.Equals(ComparisonType.ExactMatch));
		exactMatchResult?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
	}
}