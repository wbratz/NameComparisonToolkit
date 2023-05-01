using System.Collections;

namespace NameComparisonToolkit.Tests;

public class FirstLastTestData : IEnumerable<object[]>
{
	/// <summary>
	/// Only FirstName and LastName parts are compared
	/// Equals -> FirstName and LastName parts match exactly on both names
	/// EqualsIgnoreOrder -> FirstName and LastName parts match exactly but order of name parts is ignored
	/// Contains -> Name2 as a full string contains the firstName and LastName parts of Name1
	/// Intersects -> Is each name part (firstName, LastName) of Name1 in ANY name part of Name2
	/// </summary>
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"john", "Adam", "smith", "Jr.",
			"JOHN", "adam", "SMITH", "jr.",
			FirstLastMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"joan", "Adam", "smith", "Jr.",
			"JANE", "Adam", "Smith", "jr.",
			FirstLastMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"john", "A", "smith", "Jr.",
			"jon", "", "smit", "junior",
			FirstLastMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"jacob", "Adam", "SMITH", "Jr.",
			"JACOB Adam", "", "Smith", "sr.",
			FirstLastMatchResults(false, false, true, true)
		}; 
		yield return new object[]
		{
			"John James", "", "Smith Doe", "",
			"James John", "", "Doe Smith", "",
			FirstLastMatchResults(false, true, false, true)
		};
		yield return new object[]
		{
			"John James", "", "Smith Johnson", "Jr.",
			"John", "Adam", "Smith", "Jr.",
			FirstLastMatchResults(false, false, false, true)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.",
			"John James", "Adam", "Smith Johnson", "Jr.",
			FirstLastMatchResults(false, false, true, true)
		};
		yield return new object[] 
		{ 
			"John James", "", "Smith Johnson", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			FirstLastMatchResults(false, false, false, true) 
		};
		yield return new object[] 
		{ 
			"John James", "Adam", "Smith", "Jr.", 
			"jon", "", "smit", "junior", 
			FirstLastMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"jon", "", "smit", "junior", 
			"Jon James", "Adam", "Smith", "Jr.", 
			FirstLastMatchResults(false, false, true, false) 
		};
		yield return new object[] 
		{ 
			"James", "John Adam", "Smith", "Sr.", 
			"John", "Adam", "Smith", "Sr.", 
			FirstLastMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John", "James Adam", "Smith", "Jr.",
			"John", "Adam", "Smith", "Jr.", 
			FirstLastMatchResults(true, true, true, true) 
		};
		yield return new object[]
		{ 
			"James", "John", "Smith", "Sr.",
			"John", "Adam", "Smith", "sr", 
			FirstLastMatchResults(false, false, false, false) 
		};
		yield return new object[] { 
			"James", "John Adam", "Smith", "Sr.", 
			"James", "Adam", "Smith", "", 
			FirstLastMatchResults(true, true, true, true) };
		yield return new object[] 
		{ 
			"john", "Adam", "Smith", "Jr.", 
			"JOHN", "Adam James", "SMITH", "sr", 
			FirstLastMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{
			"John", "", "Smith", "Sr.", 
			"john", "Adam", "smith", "srt", 
			FirstLastMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "Jr.", 
			"John James", "Adam", "Smith", "Jr.", 
			FirstLastMatchResults(false, false, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "Jr.", 
			"JOHN", "Adam James", "SMITH", "Jr.", 
			FirstLastMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John", "Adam Smith", "James", "Jr.", 
			"John", "Adam", "Smith", "Jr.", 
			FirstLastMatchResults(false, false, false, false) 
		};
		yield return new object[] 
		{ 
			"John", "Adam", "Smith", "Jr.", 
			"John", "Adam", "Smith", "Sr.", 
			FirstLastMatchResults(true, true, true, true) 
		};
		yield return new object[] 
		{ 
			"John James", "", "Doe", "",
			"James John", "", "Smith Doe", "", 
			FirstLastMatchResults(false, false, false, true) 
		};
		
	}
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private static bool[] FirstLastMatchResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}