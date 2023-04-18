namespace NameMatching.Tests;
public class LastNameTests
{
	[Theory]
	[InlineData("John", "Adam", "Smith Doe", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Doe Smith", "Jr.", true)]
	public void Equals_ShouldHandleMultipleLastNamesCorrectly(string firstName, string middleName, string lastName, string suffix, bool expectedResult)
	{
		var name1 = new Name("John", "Adam", "Smith Doe", "Jr.");
		var name2 = new Name(firstName, middleName, lastName, suffix);

		var comparer = new Last();
		comparer.Equals(name1, name2).Should().Be(expectedResult);
	}
}
