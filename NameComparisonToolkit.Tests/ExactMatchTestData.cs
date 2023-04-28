using System.Collections;

namespace NameComparisonToolkit.Tests;

public class ExactMatchTestData : IEnumerable<object[]>
{
	/// <summary>
	/// Exact match -> each name part matches exactly from Name1 to Name2
	/// Exact Match Ignore Order -> each name part matches exactly from Name1 to Name2 - despite string order of said name part
	/// Contains -> Is each name part of Name1 contained in Name2.FullNameString
	/// Intersects -> Is each name part of Name1 in ANY name part of Name2
	/// Case Insensitive, ending . on suffix gets stripped
	/// Junior != Jr 
	/// </summary>
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "john", "adam", "smith", "jr.", ExpectedTestResults(true, true, true, true)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "john", "adam", "smith", "Jr", ExpectedTestResults(true, true, true, true)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "SR.", ExpectedTestResults(false, false, false, false)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "JANE", "adam", "smith", "Jr", ExpectedTestResults(false, false, false, false)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Junior", ExpectedTestResults(false, false, false, false)
		};
		yield return new object[]
		{
			"John", "Adam", "Smith", "Jr.", "John", "Adam", "Doe", "Jr.", ExpectedTestResults(false, false, false, false)
		};
		yield return new object[]
		{
			"John", "", "Smith Jones", "", "John", "", "SMITHJONES", "", ExpectedTestResults(false, false, false, false)
		};
		yield return new object[]
		{
			"John", "", "Smith Jones", "", "John", "", "SMITH JONES", "", ExpectedTestResults(true, true, true, true)
		};
		yield return new object[]
		{
			"John", "", "Smith", "", "John", "", "SMITH JONES", "", ExpectedTestResults(false, false, true, true)
		};
		yield return new object[]
		{
			"John", "Smith", "Jones", "", "John", "", "smith jones", "", ExpectedTestResults(false, true, true, false)
		};
		yield return new object[]
		{
			"John James", "", "Smith", "", "JAMES John", "", "smith", "", ExpectedTestResults(false, true, false, true)
		};
		yield return new object[]
		{
			"John-alton", "w", "Smith von dressel", "", "John-Alton", "W", "Smith von dressel", "", ExpectedTestResults(true, true, true, true)
		};
		yield return new object[]
		{
			"John", "Alton w", "Smith", "", "John Alton", "W", "Smith", "", ExpectedTestResults(false, true, true, true)
		};
		yield return new object[]
		{
			"John James", "Adam MICHAEL", "Smith Doe", "Jr.", "James John", "Michael Adam", "Doe Smith", "Jr.", ExpectedTestResults(false, true, false, true)
		};
		// yield return new object[]
		// {
		// 	"",ExpectedTestResults(false, false, false, false)
		// };
		// yield return new object[]
		// {
		// 	"",ExpectedTestResults(false, false, false, false)
		// };
		// yield return new object[]
		// {
		// 	"",ExpectedTestResults(false, false, false, false)
		// };

		//equalsIgnoreORder
		// [InlineData("John James", "Adam Michael", "Smith Doe", "Jr.", "James John", "Michael Adam", "Doe Smith", "Jr.", true)]
		// [InlineData("John James", "Adam Michael", "Smith Doe", "Sr.", "James John", "Michael Adam", "Smith Doe", "Sr.", true)]
		// [InlineData("John James", "Adam Michael", "Smith Doe", "Jr.", "James John", "Michael Adam", "Smith", "Jr.", false)]
		// [InlineData("John James", "Adam Michael", "Smith Doe", "Sr.", "James John", "Michael Adam", "Smith Doe", "", false)]
		// [InlineData("John", "Alton w", "Smith", "", "John Alton", "W", "Smith", "", true)]
		
		//contains
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "James", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
		// [InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam James", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", false)]
		// [InlineData("William", "", "Clayton", "", "William", "", "Clayton", "", true)]
		// [InlineData("John", "Alton w", "Smith", "", "John Alton", "W", "Smith", "", true)]
		
		//intersects
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr", true)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "James", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Sr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "James", "Adam", "Smith", "Jr.", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "", false)]
		// [InlineData("John", "Adam", "Smith", "Jr.", "John", "Adam", "Smith Johnson", "Jr.", true)]
		// [InlineData("John James", "Adam", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
		// [InlineData("John", "Adam James", "Smith", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
		// [InlineData("John", "Adam", "Smith James", "Jr.", "John", "Adam", "Smith", "Jr.", true)]
		// [InlineData("William", "", "Clayton", "", "William", "", "Clayton", "", true)]
		// [InlineData("John", "Alton w", "Smith", "", "John Alton", "W", "Smith", "", true)]
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	private static bool[] ExpectedTestResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}