﻿using NameComparisonToolkit.Extensions;
using System.Text.RegularExpressions;

namespace NameComparisonToolkit.Similarity.Algos;

internal class TokenMatcher : ISimilarityAlgorithm, IConfigurableAlgorithm
{
	private readonly Regex _tokenizer = new(@"\w+", RegexOptions.Compiled);
	private bool _useWordWeighting = false;
	private Dictionary<string, double> _wordWeights = new();

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

		var tokens1 = TokenizeString(str1);
		var tokens2 = TokenizeString(str2);

		return CalculateJaccardSimilarity(tokens1, tokens2);
	}

	private HashSet<string> TokenizeString(string input)
		=> new(
			_tokenizer.Matches(input.ToLower())
					 .Select(m => m.Value));

	public void Configure(SimilarityOptions options)
	{
		_useWordWeighting = options.UseWordWeighting;
		_wordWeights = options.CustomWordWeights;
	}

	private double CalculateJaccardSimilarity(HashSet<string> set1, HashSet<string> set2)
	{
		if (!set1.Any() && !set2.Any()) return 1.0;
		if (!set1.Any() || !set2.Any()) return 0.0;

		if (!_useWordWeighting)
		{
			var intersection = set1.Intersect(set2).Count();
			var union = set1.Union(set2).Count();
			return (double)intersection / union;
		}

		var intersectionWeight = set1.Intersect(set2)
			.Sum(token => _wordWeights.GetValueOrDefault(token, 1.0));
		var unionWeight = set1.Union(set2)
			.Sum(token => _wordWeights.GetValueOrDefault(token, 1.0));

		return intersectionWeight / unionWeight;
	}
}
