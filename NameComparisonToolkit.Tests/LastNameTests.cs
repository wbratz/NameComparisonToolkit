namespace NameComparisonToolkit.Tests;
public class LastNameTests
{
	[Theory]
	[InlineData("John", "Adam", "Smith Doe", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Doe Smith", "Jr.", false)]
	[InlineData("Joey", "Adam-levine", "smith Doe", "Junior", true)]
	public void Matches_ShouldHandleMultipleLastNamesCorrectly(string firstName, string middleName, string lastName, string suffix, bool expectedResult)
	{
		var name1 = new Name("John", "Adam", "Smith Doe", "Jr.");
		var name2 = new Name(firstName, middleName, lastName, suffix);

		var results = name1.Matches(name2);

		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John", "Adam", "Smith Doe", "Jr.", "John", "Adam", "Doe Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith Doe", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith Doe", "Jr.", "John", "Adam", "Doe", "Jr.", false)]
	public void EqualsIgnoreOrder_ShouldHandleMultipleLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.MatchesIgnoreOrder(name2);

		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "James", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam James", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	[InlineData("John", "Adam", "smith", "Jr.", "John", "Adam", "Smith von duff", "Jr", true)]
	public void Intersects_ShouldCompareLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
		
		var results = name1.Intersects(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResult);
	}
}
