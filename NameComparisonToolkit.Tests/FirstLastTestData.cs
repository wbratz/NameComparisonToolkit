using System.Collections;

namespace NameComparisonToolkit.Tests;

public class FirstLastTestData : IEnumerable<object[]>
{
	/// <summary>
	/// Only FirstName and LastName parts are compared
	/// Equals ->
	/// EqualsIgnoreOrder ->
	/// Contains ->
	/// Intersects ->
	/// </summary>
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"john", "Adam", "smith", "Jr.","JOHN", "adam", "SMITH", "jr.",  FirstLastMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"john", "Adam", "smith", "Jr.", "JANE", "Adam", "Smith", "Jr.", FirstLastMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"john", "A", "smith", "Jr.", "jon", "", "smit", "junior", FirstLastMatchResults(false, false, false, false)
		};
		yield return new object[]
		{
			"john", "Adam", "SMITH", "Jr.", "John Adam", "", "Smith", "Jr.", FirstLastMatchResults(false, true, true, false)
		};//equalIgnoreOrder T or F?
		yield return new object[]
		{
			"John James", "", "Smith Doe", "", "James John", "", "Doe Smith", "",  FirstLastMatchResults(false, true, true, true)
		};
		// yield return new object[]
		// {
		// 	"", FirstLastMatchResults(false, false, false, false)
		// };
	}
	
	
	//
	// //ignororder
	// [InlineData("John James", "Doe", "James John", "Smith Doe", false)]
	
	//
	// //contains
	// [InlineData("John James", "Adam", "Smith", "Jr.", "John James AMOS", "Adam", "Smith", "Jr.", true)]
	// [InlineData("John James", "Adam", "Smith Johnson", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
	// [InlineData("John", "Adam", "Smith", "Jr.","John James", "Adam", "Smith Johnson", "Jr.", true)]
	// [InlineData("John James", "Adam", "Smith", "Jr.","jon", "", "smit", "junior", false)]
	// [InlineData("jon", "", "smit", "junior","Jon James", "Adam", "Smith", "Jr.", true)]
	// [InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	// [InlineData("John", "James Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	// [InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	// [InlineData("James", "John Adam", "Smith", "Sr.", "James", "Adam", "Smith", "", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith James", "Jr.", true)]
	// [InlineData("John", "", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John James", "Adam", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam James", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
	
	//intersect
	// [InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	// [InlineData("James", "John Adam", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	// [InlineData("John", "James Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	// [InlineData("James", "John", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", false)]
	// [InlineData("James", "John Adam", "Smith", "Sr.", "James", "Adam", "Smith", "", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith James", "Jr.", true)]
	// [InlineData("John", "", "Smith", "Sr.", "John", "Adam", "Smith", "Sr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John James", "Adam", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam James", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
	// [InlineData("John James", "Adam", "Smith Johnson", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
	// [InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
	// [InlineData("John James", "Adam", "Smith", "Jr.","jon", "", "smit", "junior", false)]

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private static bool[] FirstLastMatchResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}