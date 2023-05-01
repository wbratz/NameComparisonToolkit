using NameComparisonToolkit.Confidence.Algos;

namespace NameComparisonToolkit.Similarity;
internal static class SimilarityBuilder
{
	internal static double Build(string name1, string name2)
		=> JaroWinkler.CalculateStringSimilarity(name1, name2);
}
