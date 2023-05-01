using System.Collections;

namespace NameComparisonToolkit.Tests;

public class LastNameTestData : IEnumerable<object[]>
{
	/// <summary>
	/// Only LastName name parts are compared
	/// Equals -> LastName parts match exactly on both names
	/// EqualsIgnoreOrder -> LastName parts match exactly but order of strings in name parts is ignored
	/// Contains -> Name2 as a full string contains the LastName part of Name1
	/// Intersects -> Is each string of name part (LastName) of Name1 in ANY name part of Name2
	/// </summary>
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"john", "Adam", "smith doe", "Jr.",
			"John", "Adam", "Smith Doe", "Jr.",
			LastNameMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"Joey", "Adam-levine", "smith Doe", "Junior",
			"John", "Adam", "Smith Doe", "Jr.",
			LastNameMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"joan", "Adam", "smith", "Jr.",
			"Smith", "Adam", "Smith Doe", "Jr.",
			LastNameMatchResults(false, false, true, true)
		};
		yield return new object[]
		{
			"john", "A", "smith", "Jr.",
			"John", "Adam", "Smith Doe", "Jr.",
			LastNameMatchResults(false, false, true, true)
		};
		yield return new object[]
		{
			"jacob", "Adam", "SMITH", "Jr.",
			"JACOB Adam", "", "Smith-Johnson", "sr.",
			LastNameMatchResults(false, false, true, false)
		}; 
		yield return new object[]
		{
			"JACOB", "Adam", "SMITH", "Jr.",
			"jacob Adam", "", "Smith", "jr.",
			LastNameMatchResults(true, true, true, true)
		}; 
		yield return new object[]
		{
			"John James", "", "Smith Doe", "",
			"James John", "", "Doe Smith", "",
			LastNameMatchResults(false, true, false, true)
		};
		yield return new object[]
		{
			"John James", "", "Smith Johnson", "Jr.",
			"John", "Adam", "Smith", "Jr.",
			LastNameMatchResults(false, false, false, true)
		};
		yield return new object[]
		{
			"John", "allen", "Smith", "Jr.",
			"John James", "Adam", "Smith Johnson", "Jr.",
			LastNameMatchResults(false, false, true, true)
		};
		yield return new object[] 
		{ 
			"John James", "", "Smith Johnson", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			LastNameMatchResults(false, false, false, true) 
		};
		yield return new object[] 
		{ 
			"John James", "Adam", "Smith", "Jr.", 
			"jon", "", "smit", "junior", 
			LastNameMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"jon", "", "smit", "junior", 
			"Jon James", "Adam", "Smith", "Jr.", 
			LastNameMatchResults(false, false, true, false) 
		};
		yield return new object[] 
		{ 
			"Jamie", "John Adam", "Smith", "Sr.", 
			"John", "Adam", "Smith", "Sr.", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[]
		{ 
			"James", "John", "Smith", "Sr.",
			"John", "Adam", "Smith", "sr", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[] { 
			"James", "John Adam", "Smith", "Sr.", 
			"james", "", "smith", "jr", 
			LastNameMatchResults(true, true, true, true) };
		yield return new object[] 
		{ 
			"john", "adam", "smith", "jr.", 
			"JOHN", "Adam James", "SMITH", "jr", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{
			"John", "", "Smith", "Sr.", 
			"john", "Adam", "smith", "srt", 
			LastNameMatchResults(true, true, true, true)  
		};
		yield return new object[] 
		{ 
			"James", "adam", "smith", "Jr.", 
			"John James", "Adam", "Smith", "jr.", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "jr", 
			"JOHN", "Adam James", "SMITH", "Jr.", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam Smith", "James", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			LastNameMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "smith VON DUFF", "Jr.",
			"James", "Adam Levine", "Smith", "Jr",
			LastNameMatchResults(false, false, false, true) 
		};
		yield return new object[] 
		{ 
			"john james", "Adam Michael", "Smith Doe", "Jr.",
			"James John", "Michael Adam", "Smith Doe", "Sr.", 
			LastNameMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"James", "Adam Levine", "Smith", "Jr",
			"John", "Adam", "smith VON DUFF", "Jr.",
			LastNameMatchResults(false, false, true, true) 
		};
	}
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private static bool[] LastNameMatchResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}
