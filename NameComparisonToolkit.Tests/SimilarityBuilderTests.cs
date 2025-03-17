using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Tests;

public class SimilarityBuilderTests
{
	[Theory]
	[InlineData("John Doe", "John Doe", 0.99)]
	[InlineData("John Doe", "Jane Doe", 0.7)]
	[InlineData("John", "Jane", 0.69)]
	[InlineData("Mary Johnson", "Mary Jane Johnson", 0.88)]
	[InlineData("Mary JOhnson", "MARy JohnsoN", 1.0)]
	[InlineData(" ", " ", 0.99)]
	[InlineData("", "", 0.99)]
	[InlineData(null, null, 0.99)]
	[InlineData("John Doe", "", 0.0)]
	[InlineData("John Doe", " ", 0.0)]
	[InlineData("John Doe", null, 0.0)]
	[InlineData("", "John Doe", 0.0)]
	[InlineData(" ", "John Doe", 0.0)]
	[InlineData(null, "John Doe", 0.0)]
	public void JaroWinkler_ReturnsCorrectSimilarityScore(string? name1, string? name2, double expectedScore)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.JaroWinkler
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		similarityScore.Should().BeApproximately(expectedScore, 0.1);
	}

	[Theory]
	[InlineData("Smith", "Smythe", 1.0)] // Same primary code
	[InlineData("Catherine", "Katherine", 1.0)] // Same primary code
	[InlineData("Gail", "Gayle", 1.0)] // Same primary code
	[InlineData("Michael", "Mikael", 1.0)] // Primary matches secondary
	[InlineData("Brian", "Bryan", 1)] // Primary matches secondary
	[InlineData("", "", 1.0)]
	[InlineData(null, null, 1.0)]
	[InlineData("Smith", "", 0.0)]
	public void DoubleMetaphone_ReturnsCorrectSimilarityScore(string? name1, string? name2, double expectedScore)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DoubleMetaphone,
			UseSecondaryEncoding = true,
			MaxEncodingLength = 4
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		similarityScore.Should().Be(expectedScore);
	}

	[Theory]
	[InlineData("John Smith", "John Smith", 1.0)]
	[InlineData("John Smith", "Smith John", 1.0)] // Order doesn't matter
	[InlineData("John Smith", "John Smythe", 0.3)] // One token matches
	[InlineData("", "", 1.0)]
	[InlineData(null, null, 1.0)]
	[InlineData("John Smith", "", 0.0)]
	public void TokenMatcher_BasicMatching_ReturnsCorrectSimilarityScore(string? name1, string? name2, double expectedScore)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.TokenMatcher,
			UseWordWeighting = false
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		similarityScore.Should().BeApproximately(expectedScore, 0.1);
	}

	[Fact]
	public void TokenMatcher_WithWeights_ReturnsCorrectSimilarityScore()
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.TokenMatcher,
			UseWordWeighting = true,
			CustomWordWeights = new Dictionary<string, double>
			{
				["smith"] = 2.0,
				["john"] = 1.0
			}
		};

		// Act
		var score1 = SimilarityBuilder.Build("John Smith", "John Smith", options);
		var score2 = SimilarityBuilder.Build("John Smith", "John Jones", options);

		// Assert
		score1.Should().Be(1.0);
		score2.Should().BeLessThan(0.5); // Smith has higher weight, so missing it reduces score more
	}

	[Theory]
	[InlineData("kitten", "sitting", 0.57)] // 3 operations: k->s, e->i, +g
	[InlineData("saturday", "sunday", 0.625)] // 3 operations: remove 'at', 'r'
	[InlineData("", "", 1.0)]
	[InlineData(null, null, 1.0)]
	[InlineData("hello", "", 0.0)]
	public void DamerauLevenshtein_Normalized_ReturnsCorrectSimilarityScore(string? name1, string? name2, double expectedScore)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DamerauLevenshtein,
			NormalizeDistance = true,
			TranspositionCost = 1.0
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		similarityScore.Should().BeApproximately(expectedScore, 0.01);
	}

	[Theory]
	[InlineData("book", "back", false, 2)] // 2 operations without normalization
	[InlineData("book", "back", true, 0.5)] // Same case normalized to 0-1 range
	public void DamerauLevenshtein_NormalizationSetting_AffectsScore(
		string name1, string name2, bool normalize, double expectedScore)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DamerauLevenshtein,
			NormalizeDistance = normalize,
			TranspositionCost = 1.0
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		if (normalize)
			similarityScore.Should().BeApproximately(expectedScore, 0.01);
		else
			similarityScore.Should().Be(expectedScore == 0 ? 1.0 : 0.0);
	}

	[Theory]
	[InlineData("book", "bokk", 1.0)] // Standard cost
	[InlineData("book", "bokk", 0.5)] // Half cost
	public void DamerauLevenshtein_TranspositionCost_AffectsScore(string name1, string name2, double transpositionCost)
	{
		// Arrange
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DamerauLevenshtein,
			NormalizeDistance = true,
			TranspositionCost = transpositionCost
		};

		// Act
		var similarityScore = SimilarityBuilder.Build(name1, name2, options);

		// Assert
		similarityScore.Should().BeGreaterThan(0.7); // Transposition should result in high similarity
	}
}
