using NameComparisonToolkit.Confidence.Algos;

namespace NameComparisonToolkit.Confidence;
public static class ConfidenceBuilder
{
	public static double Build(string name1, string name2)
		=> JaroWinkler.CalculateStringSimilarity(name1, name2);
}
