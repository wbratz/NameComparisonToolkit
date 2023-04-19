namespace NameMatching.Tests;

public class FirstLastTests
{
	[Theory]
	[InlineData("John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Sr.", true)]
	[InlineData("Jane", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Eve", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Doe", "Jr.", false)]
	[InlineData("John Adam", "", "Smith", "Jr.", true)]
	[InlineData("J", "", "Smith", "Jr.", false)]
	public void Equals_ShouldCompareFirstNameAndLastNameCorrectly(string firstName, string middleName, string lastName, string suffix, bool expectedResult)
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name(firstName, middleName, lastName, suffix);

		var comparer = new FirstLast();
		comparer.Equals(name1, name2).Should().Be(expectedResult);
	}
}

