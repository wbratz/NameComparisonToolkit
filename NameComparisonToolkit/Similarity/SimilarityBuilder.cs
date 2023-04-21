using NameComparisonToolkit.Confidence.Algos;

namespace NameComparisonToolkit.Similarity;
public static class SimilarityBuilder
{
	public static double Build(string name1, string name2)
		=> JaroWinkler.CalculateStringSimilarity(name1, name2);
}
