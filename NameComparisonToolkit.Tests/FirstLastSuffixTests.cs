namespace NameComparisonToolkit.Tests;
public class FirstLastSuffixTests
{
	[Theory]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "Jane", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Eve", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Doe", "Jr.", false)]
	[InlineData("John", "", "Smith Jones", "", "John", "", "Smith Jones", "", true)]
	[InlineData("John", "D", "Smith Jones", "", "John", "", "Jones Smith", "", true)]
	[InlineData("John", "", "Smith", "", "John", "", "Smith Jones", "", false)]
	public void Equals_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var comparer = new FirstLastSuffix();
		comparer.Equals(name1, name2).Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "James Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith James", "Jr.", true)]
	[InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "James", "Sr.", false)]
	[InlineData("John", "", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John James", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "James Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam James", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith Johnson", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]

	public void Contains_ShouldCompareFirstLastSuffixCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var comparer = new FirstLastSuffix();
		comparer.Contains(name1, name2).Should().Be(expectedResult);
	}
}
