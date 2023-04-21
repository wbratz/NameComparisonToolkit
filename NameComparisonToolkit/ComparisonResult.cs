namespace NameComparisonToolkit;

public sealed record ComparisonResult(string Method, bool IsMatch, double Similarity);
public sealed record ComparisonResults(IEnumerable<ComparisonResult> Results, string Error);
