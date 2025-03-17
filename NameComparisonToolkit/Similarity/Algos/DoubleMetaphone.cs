using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Similarity.Algos;

internal class DoubleMetaphone : ISimilarityAlgorithm, IConfigurableAlgorithm
{
	private bool _useSecondaryEncoding = true;
	private int _maxEncodingLength = 4;

	public void Configure(SimilarityOptions options)
	{
		_useSecondaryEncoding = options.UseSecondaryEncoding;
		_maxEncodingLength = options.MaxEncodingLength;
	}

	public double Calculate(string str1, string str2)
	{
		if (str1.IsNullOrWhiteSpace() && str2.IsNullOrWhiteSpace())
		{
			return 1.0;
		}

		if (str1.IsNullOrWhiteSpace() || str2.IsNullOrWhiteSpace())
		{
			return 0.0;
		}

		var meta1 = str1.ToDoubleMetaphone();
		var meta2 = str2.ToDoubleMetaphone();

		var result = string.Equals(meta1, meta2, StringComparison.OrdinalIgnoreCase);

		if (!result)
		{
			result = str1.CompareDoubleMetaphoneAdvanced(str2);
		}

		return result ? 1.0 : 0.0;
	}
}