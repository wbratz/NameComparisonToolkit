namespace NameMatching.Tests;
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

		var comparer = new ExactMatch();
		comparer.Equals(name1, name2).Should().Be(expectedResult);
	}
}
