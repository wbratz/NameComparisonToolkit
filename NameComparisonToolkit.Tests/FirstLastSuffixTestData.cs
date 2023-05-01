using System.Collections;

namespace NameComparisonToolkit.Tests;

public class FirstLastSuffixTestData : IEnumerable<object[]>
{
	/// <summary>
	/// Only FirstName, LastName, Suffix parts are compared
	/// Equals -> FirstName and LastName and Suffix parts match exactly on both names
	/// EqualsIgnoreOrder -> FirstName and LastName and Suffix parts match exactly but order of strings in name parts is ignored
	/// Contains -> Name2 as a full string contains the firstName and LastName and Suffix parts of Name1
	/// Intersects -> Is each name part (firstName, LastName, Suffix) of Name1 in ANY name part of Name2
	/// </summary>
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"john", "Adam", "smith", "Jr.",
			"JOHN", "adam", "SMITH", "jr.",
			FirstLastSuffixMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"joan", "Adam", "smith", "Jr.",
			"JANE", "Adam", "Smith", "jr.",
			FirstLastSuffixMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"john", "A", "smith", "Jr.",
			"jon", "", "smit", "junior",
			FirstLastSuffixMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"jacob", "Adam", "SMITH", "Jr.",
			"JACOB Adam", "", "Smith", "sr.",
			FirstLastSuffixMatchResults(false, false, false, false)
		}; 
		yield return new object[]
		{
			"JACOB", "Adam", "SMITH", "Jr.",
			"jacob Adam", "", "Smith", "jr.",
			FirstLastSuffixMatchResults(false, true, true, true)
		}; 
		yield return new object[]
		{
			"John James", "", "Smith Doe", "",
			"James John", "", "Doe Smith", "",
			FirstLastSuffixMatchResults(false, true, false, true)
		};
		yield return new object[]
		{
			"John James", "", "Smith Johnson", "Jr.",
			"John", "Adam", "Smith", "Jr.",
			FirstLastSuffixMatchResults(false, false, false, true)
		};
		yield return new object[]
		{
			"John", "allen", "Smith", "Jr.",
			"John James", "Adam", "Smith Johnson", "Jr.",
			FirstLastSuffixMatchResults(false, false, true, true)
		};
		yield return new object[] 
		{ 
			"John James", "", "Smith Johnson", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			FirstLastSuffixMatchResults(false, false, false, true) 
		};
		yield return new object[] 
		{ 
			"John James", "Adam", "Smith", "Jr.", 
			"jon", "", "smit", "junior", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"jon", "", "smit", "junior", 
			"Jon James", "Adam", "Smith", "Jr.", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"Jamie", "John Adam", "Smith", "Sr.", 
			"John", "Adam", "Smith", "Sr.", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John", "James Adam", "Smith", "Jr.",
			"John", "Adam", "Smith", "Jr.", 
			FirstLastSuffixMatchResults(true, true, true, true) 
		};
		yield return new object[]
		{ 
			"James", "John", "Smith", "Sr.",
			"John", "Adam", "Smith", "sr", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] { 
			"James", "John Adam", "Smith", "Sr.", 
			"james", "adam", "smith", "sr", 
			FirstLastSuffixMatchResults(true, true, true, true) };
		yield return new object[] 
		{ 
			"john", "adam", "smith", "jr.", 
			"JOHN", "Adam James", "SMITH", "jr", 
			FirstLastSuffixMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{
			"John", "", "Smith", "Sr.", 
			"john", "Adam", "smith", "srt", 
			FirstLastSuffixMatchResults(false, false, true, false) 
		};
		yield return new object[] 
		{ 
			"James", "adam", "smith", "Jr.", 
			"John James", "Adam", "Smith", "jr.", 
			FirstLastSuffixMatchResults(false, false, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "jr", 
			"JOHN", "Adam James", "SMITH", "Jr.", 
			FirstLastSuffixMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam Smith", "James", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "Jr.", 
			"John", "Adam", "Smith", "Sr.", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"William", "", "Clayton", "",
			"William", "A", "Clayton", "", 
			FirstLastSuffixMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"john james", "adam michael", "Smith Doe", "jr",
			"James John", "Michael Adam", "Doe Smith", "Jr.",
			FirstLastSuffixMatchResults(false, true, false, true) 
		};
		yield return new object[] 
		{ 
			"john james", "Adam Michael", "Smith Doe", "Jr.",
			"James John", "Michael Adam", "Smith Doe", "Sr.", 
			FirstLastSuffixMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John James", "Adam Michael", "Smith Doe", "",
			"James John", "Michael Adam", "Smith Doe", "",
			FirstLastSuffixMatchResults(false, true, false, true) 
		};
	}
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private static bool[] FirstLastSuffixMatchResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}