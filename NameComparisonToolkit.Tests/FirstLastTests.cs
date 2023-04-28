﻿namespace NameComparisonToolkit.Tests;

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
	[InlineData("john", "", "smith", "junior", true)]
	[InlineData("jon", "", "smit", "junior", false)]
	public void Equals_ShouldCompareFirstNameAndLastNameCorrectly(string firstName, string middleName, string lastName, string suffix, bool expectedResult)
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name(firstName, middleName, lastName, suffix);

		var allResults = name1.Matches(name2).ToList();
		
		var  results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLast));
		results?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John James", "Smith Doe", "James John", "Doe Smith", true)]
	[InlineData("John James", "Smith Doe", "James John", "Smith Doe", true)]
	[InlineData("John James", "Doe", "James John", "Smith Doe", false)]
	[InlineData("jon", "", "smit", "junior", false)]
	public void EqualsIgnoreOrder_ShouldCompareCorrectly(string firstName1, string lastName1, string firstName2, string lastName2, bool expectedResult)
	{
		var name1 = new Name(firstName1, lastName1);
		var name2 = new Name(firstName2, lastName2);
		var allResults = name1.MatchesIgnoreOrder(name2).ToList();
		
		var  results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLast));
		results?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("John James", "Adam", "Smith", "Jr.", "John James AMOS", "Adam", "Smith", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith Johnson", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.","John James", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith", "Jr.","jon", "", "smit", "junior", false)]
	[InlineData("jon", "", "smit", "junior","Jon James", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "James Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "James", "Adam", "Smith", "", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith James", "Jr.", true)]
	[InlineData("John", "", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John James", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam James", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
	public void Contains_ShouldCompareFirstNameAndLastNameCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.Contains(name2.GetFullName()).ToList();
		
		var  results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLast));
		results?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
		
	}
	
	[Theory]
	[InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("John", "James Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	[InlineData("James", "John Adam", "Smith", "Sr.", "James", "Adam", "Smith", "", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith James", "Jr.", true)]
	[InlineData("John", "", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John James", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam James", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	[InlineData("John James", "Adam", "Smith Johnson", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	[InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
	[InlineData("John James", "Adam", "Smith", "Jr.","jon", "", "smit", "junior", false)]
	public void Intersects_ShouldCompareFirstNameAndLastNameCorrectly(string firstName1, string middleName1, string lastName1, string suffix1, string firstName2, string middleName2, string lastName2, string suffix2, bool expectedResult)
	{
		var name1 = new Name(firstName1, middleName1, lastName1, suffix1);
		var name2 = new Name(firstName2, middleName2, lastName2, suffix2);

		var allResults = name1.Intersects(name2).ToList();
		
		var  results = allResults.Where(x => x.ComparisonType.Equals(ComparisonType.FirstLast));
		results?.FirstOrDefault()?.IsMatch.Should().Be(expectedResult);
		
	}
}

