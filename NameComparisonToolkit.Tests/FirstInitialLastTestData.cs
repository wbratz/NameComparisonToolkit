using System.Collections;

namespace NameComparisonToolkit.Tests;
public class FirstInitialLastTestData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[]
		{
			"john", "Adam", "smith", "Jr.",
			"JOHN", "adam", "SMITH", "jr.",
			FirstInitialLastMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"joan", "Adam", "smith", "Jr.",
			"JANE", "Adam", "Smith", "jr.",
			FirstInitialLastMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"john", "A", "smith", "Jr.",
			"jack", "", "smith", "junior",
			FirstInitialLastMatchResults(true, true, true, true)
		};
		yield return new object[]
		{
			"jacob", "Adam", "SMITH", "Jr.",
			"JACOB Adam", "", "Smith", "sr.",
			FirstInitialLastMatchResults(false, true, true, true)
		};
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private static bool[] FirstInitialLastMatchResults(bool equalOutcome, bool equalIgnoreOrderOutcome, bool containsOutcome, bool intersectsOutcome)
	{
		return new[] { equalOutcome, equalIgnoreOrderOutcome, containsOutcome, intersectsOutcome };
	}
}
