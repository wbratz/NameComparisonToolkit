using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Similarity.Algos;

internal class DamerauLevenshtein : ISimilarityAlgorithm, IConfigurableAlgorithm
{
	private bool _normalizeDistance = true;
	private double _transpositionCost = 1.0;

	public void Configure(SimilarityOptions options)
	{
		_normalizeDistance = options.NormalizeDistance;
		_transpositionCost = options.TranspositionCost;
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

		var distance = CalculateDistance(str1, str2);
		var maxLength = Math.Max(str1.Length, str2.Length);

		if (_normalizeDistance)
		{
			return 1.0 - ((double)distance / maxLength);
		}

		return distance == 0 ? 1.0 : 0.0;
	}

	private int CalculateDistance(string str1, string str2)
	{
		var len1 = str1.Length;
		var len2 = str2.Length;
		var matrix = new int[len1 + 1, len2 + 1];

		// Initialize first row and column
		for (var i = 0; i <= len1; i++) matrix[i, 0] = i;
		for (var j = 0; j <= len2; j++) matrix[0, j] = j;

		for (var i = 1; i <= len1; i++)
		{
			for (var j = 1; j <= len2; j++)
			{
				var cost = str1[i - 1] == str2[j - 1] ? 0 : 1;

				matrix[i, j] = Math.Min(Math.Min(
					matrix[i - 1, j] + 1,      // deletion
					matrix[i, j - 1] + 1),     // insertion
					matrix[i - 1, j - 1] + cost // substitution
				);

				// Check for transposition
				if (i > 1 && j > 1 &&
					str1[i - 1] == str2[j - 2] &&
					str1[i - 2] == str2[j - 1])
				{
					matrix[i, j] = Math.Min(
						matrix[i, j],
					matrix[i - 2, j - 2] + (int)(_transpositionCost * cost) // transposition with configurable cost
					);
				}
			}
		}

		return matrix[len1, len2];
	}
}
