namespace NameComparisonToolkit.Tests;
public class SimilarityBuilderTests
{
	[Theory]
	[InlineData("John Doe", "John Doe", .99)]
	[InlineData("John Doe", "Jane Doe", 0.7)]
	[InlineData("John", "Jane", 0.69)]
	[InlineData("Mary Johnson", "Mary Jane Johnson", 0.88)]
	[InlineData("Mary JOhnson", "MARy JohnsoN", 0.71)]
	[InlineData(" ", " ", .99)]
	[InlineData("", "", .99)]
	[InlineData(null, null, .99)]
	[InlineData("John Doe", "", -0.1)]
	[InlineData("John Doe", " ", -0.1)]
	[InlineData("John Doe", null, -0.1)]
	[InlineData("", "John Doe", -0.1)]
	[InlineData(" ", "John Doe", -0.1)]
	[InlineData(null, "John Doe", -0.1)]
	public void Build_ReturnsCorrectSimilarityScore(string name1, string name2, double lowerBound)
	{
		//var name = 

		// Act
		//var similarityScore = SimilarityBuilder.Build(name1, name2);

		//// Assert
		//similarityScore.Should().BeGreaterThan(lowerBound);
	}
}
