namespace NameComparisonToolkit;

public sealed record ComparisonResult(ComparisonType ComparisonType, bool IsMatch, double Similarity);
public sealed record ComparisonResults(IEnumerable<ComparisonResult> Results, string Error); //can we remove this?
