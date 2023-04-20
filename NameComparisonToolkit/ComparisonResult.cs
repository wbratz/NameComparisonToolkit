namespace NameComparisonToolkit;

public sealed class ComparisonResult
{
	public string Method { get; set; }
	public bool IsMatch { get; set; }
	public float Confidence { get; set; }
}

public sealed class ComparisonResults
{
	public IEnumerable<ComparisonResult> Results { get; set; }
	public string Error { get; set; }
}
