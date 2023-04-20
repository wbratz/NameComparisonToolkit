﻿namespace NameComparisonToolkit.Tests;
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
	public void Contains_ShouldCompareLastNamesCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var comparer = new Last();
		comparer.Contains(name1, name2).Should().Be(expectedResult);
	}
}