﻿﻿﻿using NameComparisonToolkit.Similarity.Algos;

namespace NameComparisonToolkit.Similarity;
public static class SimilarityBuilder
{
	private static readonly Dictionary<SimilarityAlgorithmType, ISimilarityAlgorithm> _algorithms =
			new()
			{
			{ SimilarityAlgorithmType.JaroWinkler, new JaroWinkler() },
			{ SimilarityAlgorithmType.DoubleMetaphone, new DoubleMetaphone() },
			{ SimilarityAlgorithmType.TokenMatcher, new TokenMatcher() },
			{ SimilarityAlgorithmType.DamerauLevenshtein, new DamerauLevenshtein() }
			};

	public static double Build(string name1, string name2, SimilarityOptions? options = null)
	{
		options ??= new SimilarityOptions();

		var str1 = PreprocessString(name1, options);
		var str2 = PreprocessString(name2, options);

		var algorithm = _algorithms[options.Algorithm];

		// Pass options to algorithm if it implements IConfigurableAlgorithm
		if (algorithm is IConfigurableAlgorithm configurable)
		{
			configurable.Configure(options);
		}

		return algorithm.Calculate(str1, str2);
	}

	private static string PreprocessString(string input, SimilarityOptions options)
	{
		if (string.IsNullOrEmpty(input)) return string.Empty;

		var result = input;

		if (options.CaseInsensitive)
			result = result.ToLowerInvariant();

		if (options.IgnoreWhitespace)
			result = result.Trim();

		return result;
	}
}
