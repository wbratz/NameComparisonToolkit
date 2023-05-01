namespace NameComparisonToolkit.Tests;
public class LastNameTests
{
	[Theory]
	[ClassData(typeof(LastNameTestData))]
	public void Equals_ShouldHandleMultipleLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.Matches(name2);

		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResults[0]);
	}

	[Theory]
	[ClassData(typeof(LastNameTestData))]
	public void EqualsIgnoreOrder_ShouldHandleMultipleLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var results = name1.MatchesIgnoreOrder(name2);

		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResults[1]);
	}
	[Theory]
	[ClassData(typeof(LastNameTestData))]
	public void Contains_ShouldCompareLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
		
		var results = name1.Contains(name2.GetFullName());
		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResults[2]);
	}
	

	[Theory]
	[ClassData(typeof(LastNameTestData))]
	public void Intersects_ShouldCompareLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool[] expectedResults)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);
		
		var results = name1.Intersects(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.Last)).IsMatch.Should().Be(expectedResults[3]);
	}
}
