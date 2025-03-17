namespace NameComparisonToolkit.Similarity;

public class SimilarityOptions
{
	public SimilarityAlgorithmType Algorithm { get; set; } = SimilarityAlgorithmType.JaroWinkler;

	public bool CaseInsensitive { get; set; } = true;
	public bool IgnoreWhitespace { get; set; } = true;

	public bool UseWordWeighting { get; set; } = false;
	public Dictionary<string, double> CustomWordWeights { get; set; } = new();

	public bool UseSecondaryEncoding { get; set; } = true;
	public int MaxEncodingLength { get; set; } = 4;

	public bool NormalizeDistance { get; set; } = true;
	public double TranspositionCost { get; set; } = 1.0;
}