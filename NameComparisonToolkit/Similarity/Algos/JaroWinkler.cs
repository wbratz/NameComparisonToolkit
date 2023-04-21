using NameComparisonToolkit.Extensions;

namespace NameComparisonToolkit.Confidence.Algos;
internal static class JaroWinkler
{
	internal static double CalculateStringSimilarity(string str1, string str2)
	{
		if (str1.IsNullOrWhiteSpace() && str2.IsNullOrWhiteSpace())
		{
			return 1.0;
		}

		if (str1.IsNullOrWhiteSpace() || str2.IsNullOrWhiteSpace())
		{
			return 0.0;
		}

		var matches = 0;
		var transpositions = 0;
		var halfLength = Math.Min(str1.Length, str2.Length) / 2 + 1;

		var matched1 = new bool[str1.Length];
		var matched2 = new bool[str2.Length];

		for (var i = 0; i < str1.Length; i++)
		{
			var start = Math.Max(0, i - halfLength + 1);
			var end = Math.Min(i + halfLength, str2.Length);

			for (var j = start; j < end; j++)
			{
				if (matched2[j])
				{
					continue;
				}

				if (str1[i] != str2[j])
				{
					continue;
				}

				matched1[i] = true;
				matched2[j] = true;

				matches++;
				break;
			}
		}

		if (matches == 0)
		{
			return 0.0;
		}

		var k = 0;
		for (var i = 0; i < str1.Length; i++)
		{
			if (!matched1[i])
			{
				continue;
			}

			while (!matched2[k])
			{
				k++;
			}

			if (str1[i] != str2[k])
			{
				transpositions++;
			}

			k++;
		}

		var jaro = CalculateJaro(matches, str1.Length, str2.Length, transpositions);
		var prefix = 0;

		for (var i = 0; i < Math.Min(4, Math.Min(str1.Length, str2.Length)); i++)
		{
			if (str1[i] == str2[i])
			{
				prefix++;
			}
			else
			{
				break;
			}
		}

		return jaro + 0.1 * prefix * (1 - jaro);
	}

	private static double CalculateJaro(int matches, int str1Len, int str2Len, int transpositions)
		=> ((double)matches / str1Len + (double)matches / str2Len + (double)(matches - transpositions / 2) / matches) / 3;
}
