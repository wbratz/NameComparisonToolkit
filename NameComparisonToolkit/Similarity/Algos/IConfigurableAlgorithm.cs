namespace NameComparisonToolkit.Similarity.Algos;

internal interface IConfigurableAlgorithm : ISimilarityAlgorithm
{
	void Configure(SimilarityOptions options);
}
