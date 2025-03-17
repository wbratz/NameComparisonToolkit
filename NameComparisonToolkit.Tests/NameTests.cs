using NameComparisonToolkit.Similarity;

namespace NameComparisonToolkit.Tests;

public class NameTests
{
	[Fact]
	public void GetTokenizedName_ShouldReturnCorrectTokens()
	{
		var name = new Name("John", "Adam", "Smith", "Jr.");
		var tokensWithSuffix = name.GetTokenizedName(includeSuffix: true);
		var tokensWithoutSuffix = name.GetTokenizedName(includeSuffix: false);

		tokensWithSuffix.Should().Equal(new List<string> { "john", "adam", "smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "john", "adam", "smith" });
		tokensWithSuffix.Should().Equal(new List<string> { "john", "adam", "smith", "jr" });
		tokensWithoutSuffix.Should().Equal(new List<string> { "john", "adam", "smith" });
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMatchingNames_AllMatchTypes()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnTrueForMixedSuffixPunctuation()
	{
		var name1 = new Name("john", "Adam", "Smith", "Jr");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeTrue();
	}

	[Fact]
	public void Matches_ShouldReturnFalseForNonMatchingNames()
	{
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Doe", "Jr.");

		var results = name1.Matches(name2);
		results.First(x => x.ComparisonType.Equals(ComparisonType.ExactMatch)).IsMatch.Should().BeFalse();
	}


	[Fact]
	public void GetMatchResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.Matches(name2);

		result.Count().Should().Be(typeCount);
	}

	[Fact]
	public void GetContainResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");

		var result = name1.Contains("name");

		result.Count().Should().Be(typeCount);
	}

	[Fact]
	public void GetIntersectResults_ShouldReturnResultCount_EqualToComparisonTypeCount()
	{
		var typeCount = Enum.GetNames(typeof(ComparisonType)).Length;
		var name1 = new Name("John", "Adam", "Smith", "Jr.");
		var name2 = new Name("John Adam", "Joel", "Smith", "Jr.");

		var result = name1.Intersects(name2);

		result.Count().Should().Be(typeCount);
	}

	[Fact]
	public void TryParse_FirstLastSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("John Smith III");
		name.GetFullName().Should().BeEquivalentTo("John Smith III");
	}

	[Fact]
	public void TryParse_FirstTwoPartLastSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("John michael Van DER berg III");
		name.GetFullName().Should().BeEquivalentTo("John michael Van der berg III");
	}
	
	[Fact]
	public void TryParse_FirstMiddleLastSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("John Adams Smith III");
		name.GetFullName().Should().BeEquivalentTo("John Adams Smith III");
	}
	
	[Fact]
	public void TryParse_LastCommaFirst_ParsesCorrectly()
	{
		var name = Name.TryParse("Smith, John adam III");
		name.GetFullName().Should().BeEquivalentTo("John Adam Smith III");
	}
	
	[Fact]
	public void TryParse_LastWithSpaceCommaFirstMiddleLastSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("Van Lew, Adam bosh sr");
		name.GetFullName().Should().BeEquivalentTo("adam bosh van lew sr");
	}
	
	[Fact]
	public void TryParse_LastCommaFirstWithMiddleExtraComma_ParsesCorrectly()
	{
		var name = Name.TryParse("Smith-johnson, John adam, III");
		name.GetFullName().Should().BeEquivalentTo("John Adam Smith-JohnsON III");
	}
	
	[Fact]
	public void TryParse_LastCommaFirstWithNoSpace_ParsesCorrectly()
	{
		var name = Name.TryParse("Smith,John adam III");
		name.GetFullName().Should().BeEquivalentTo("Smith,JOHN adam III");
	}
	
	[Fact]
	public void TryParse_NameCommaSpace_ParsesCorrectly()
	{
		var name = Name.TryParse("Smith, ");
		name.GetFullName().Should().BeEquivalentTo("Smith,");
	}
	
	[Fact]
	public void TryParse_LastNameCommaFirstWithMiddleNoSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("Kerr, Katelyn R");
		name.GetFullName().Should().BeEquivalentTo("Katelyn R Kerr");
	}
	
	[Fact]
	public void TryParse_LastNameCommaFirstWithMiddleWithSuffix_ParsesCorrectly()
	{
		var name = Name.TryParse("Kerr, Katelyn, R Jr");
		name.GetFullName().Should().BeEquivalentTo("Katelyn R Kerr Jr");
	}
	
	[Fact]
	public void TryParse_LastNameCommaFirstWithMiddleExtraComma_ParsesCorrectly()
	{
		var name = Name.TryParse("Kerr, Katelyn R, ");
		name.GetFullName().Should().BeEquivalentTo("Katelyn R Kerr");
	}
	
	[Theory]
	[InlineData(SimilarityAlgorithmType.JaroWinkler)]
	[InlineData(SimilarityAlgorithmType.DoubleMetaphone)]
	[InlineData(SimilarityAlgorithmType.TokenMatcher)]
	[InlineData(SimilarityAlgorithmType.DamerauLevenshtein)]
	public void GetSimilarity_WithName_ShouldReturnResultsForAllComparisonTypes(SimilarityAlgorithmType algorithm)
	{
		// Arrange
		var name1 = new Name("John", "Michael", "Smith", "Jr");
		var name2 = new Name("Jon", "Mike", "Smythe", "Jr");
		var options = new SimilarityOptions { Algorithm = algorithm };

		// Act
		var results = name1.Matches(name2, options);

		// Assert
		results.Count().Should().Be(Enum.GetValues(typeof(ComparisonType)).Length);
		results.All(r => r.Similarity >= 0.0 && r.Similarity <= 1.0).Should().BeTrue();
	}

	[Fact]
	public void GetSimilarity_WithDoubleMetaphone_ShouldHandlePhoneticVariations()
	{
		// Arrange
		var name1 = new Name("Catherine", "", "Smith", "");
		var name2 = new Name("Katherine", "", "Smythe", "");
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DoubleMetaphone,
			UseSecondaryEncoding = true
		};

		// Act
		var results = name1.Matches(name2, options);

		// Assert
		foreach (var result in results)
		{
			result.Similarity.Should().BeGreaterThan(0.8);
		}
	}

	[Fact]
	public void GetSimilarity_WithTokenMatcher_ShouldHandleWordOrder()
	{
		// Arrange
		var name1 = new Name("John Michael", "", "Smith", "");
		var name2 = new Name("Michael John", "", "Smith", "");
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.TokenMatcher,
			UseWordWeighting = false
		};

		// Act
		var results = name1.Matches(name2, options);

		// Assert
		foreach (var result in results)
		{
			result.Similarity.Should().BeGreaterThan(0.8);
		}
	}

	[Fact]
	public void GetSimilarity_WithDamerauLevenshtein_ShouldHandleTypos()
	{
		// Arrange
		var name1 = new Name("John", "", "Smith", "");
		var name2 = new Name("Jhon", "", "Simth", "");
		var options = new SimilarityOptions
		{
			Algorithm = SimilarityAlgorithmType.DamerauLevenshtein,
			NormalizeDistance = true
		};

		// Act
		var results = name1.Matches(name2, options).ToList();

		// Assert
		foreach (var result in results)
		{
			result.Similarity.Should().BeGreaterThan(0.59);
		}
	}

	[Fact]
	public void GetSimilarity_WithNullOrEmpty_ShouldHandleGracefully()
	{
		// Arrange
		var name1 = new Name("John", "", "Smith", "");
		var name2 = new Name("", "", "", "");
		var options = new SimilarityOptions();

		// Act
		var results = name1.Matches(name2, options);

		// Assert
		results.Should().NotBeNull();
		results.All(r => r.Similarity >= 0.0 && r.Similarity <= 1.0).Should().BeTrue();
	}
}