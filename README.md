# NameComparisonToolkit

A powerful and flexible .NET library for comparing names in a variety of ways. It allows you to compare names using different comparison types, as well as providing useful methods for tokenizing, containing, and intersecting names.

## Table of Contents

- [Features](#features)
- [Usage](#usage)
  - [Comparing Names](#comparing-names)
  - [Containing Names](#containing-names)
  - [Intersecting Names](#intersecting-names)
  - [Comparison Results](#comparison-results)
  - [Similarity Score](#similarity-score)
- [Additional Features](#additional-features)
- [Installation](#installation)
- [License](#license)

## Features

- Compare names using a variety of comparison types
- Generate full names from individual name components (first, middle, last, and suffix)
- Retrieve a tokenized version of a name as a list of strings
- Check if a name is contained within another name
- Check if a name intersects with another name
- Calculate similarity between names

## Usage

First, import the library:
```csharp
using NameComparisonToolkit;
```

You can create a `Name` object in two ways:

1. Using the constructor:
```csharp
var name1 = new Name("John", "Doe");
var name2 = new Name("John", "Michael", "Smith", "Jr.");  // with middle name and suffix
```

2. Using TryParse for full name strings:
```csharp
// Space-separated format (assumes "first middle last suffix" order)
var name1 = Name.TryParse("John Michael Smith Jr");

// Comma-separated format (assumes "last, first middle suffix" order)
var name2 = Name.TryParse("Smith, John Michael Jr");
```

The `TryParse` method is particularly useful when working with full name strings. It supports:
- Space-separated names (interpreted as "first middle last suffix")
- Comma-separated names (interpreted as "last, first middle suffix")
  - Note: Only commas followed by a space (", ") trigger the "last, first" format
  - Commas without a following space are treated as part of the name (e.g., "Smith,Jr" is not split)
  - Additional commas are ignored 
- Names with or without middle names
- Names with or without suffixes (Jr, Sr, III, etc.)
- Multiple word last names

Important: When parsing names with commas:
- "Smith, John" -> Last: "Smith", First: "John" (comma + space triggers last,first format)
- "Smith,John" -> First: "Smith,John" (no space after comma, treated as single name)

Examples:
```csharp
// Basic first and last
var name1 = Name.TryParse("John Smith");                    // First: "John", Last: "Smith"

// With middle name
var name2 = Name.TryParse("John Michael Smith");            // First: "John", Middle: "Michael", Last: "Smith"

// With suffix
var name3 = Name.TryParse("John Michael Smith Jr");         // First: "John", Middle: "Michael", Last: "Smith", Suffix: "Jr"

// Multi-word last name without comma
var name4 = Name.TryParse("John Van Der Berg");             // First: "John", Middle: "Van", Last: "Der Berg"
                                                           // But name4.GetFullName() still returns "john van der berg"

// With comma (preferred for multi-word last names)
var name5 = Name.TryParse("Van Der Berg, John");            // First: "John", Last: "Van Der Berg"
                                                           // name5.GetFullName() returns "john van der berg"

// Note: For space-separated multi-word last names, using comma format is recommended
// to ensure correct parsing of the last name as a single unit
```

### Comparing Names

Compare the two names using all available comparison types:

```csharp
var name1 = new Name("John Jacob", "Doe Smith");
var name2 = new Name("John Jacob", "Smith Doe");

var matches = name1.Matches(name2);

foreach (var result in matches)
{
    Console.WriteLine($"Comparison Type: {result.ComparisonType}");
    Console.WriteLine($"Is Match: {result.IsMatch}");
    Console.WriteLine($"Similarity: {result.Similarity}");
}
```

Example output:

```json
"ComparisonType": "Exact"
"IsMatch": false
"Similarity": 0.5

```

Compare the two names ignoring the order of the name parts:
```csharp
var name1 = new Name("John Jacob", "Doe Smith");
var name2 = new Name("Jacob John", "Smith Doe");

var matchesIgnoreOrder = name1.MatchesIgnoreOrder(name2);

foreach (var result in matchesIgnoreOrder)
{
    Console.WriteLine($"Comparison Type: {result.ComparisonType}");
    Console.WriteLine($"Is Match: {result.IsMatch}");
    Console.WriteLine($"Similarity: {result.Similarity}");
}
```

Example output:

```json
"ComparisonType": "Exact"
"IsMatch": true
"Similarity": 1.0

```

### Containing Names
Check if the current name object is contained within the given name string:

```csharp
var name1 = new Name("John Jacob", "Doe Smith");

var containsResult = name1.Contains("John Jacob Doe Smith is a test subject");

Console.WriteLine($"Comparison Type: {containsResult.ComparisonType}");
Console.WriteLine($"Contains: {containsResult.IsMatch}");
Console.WriteLine($"Similarity: {containsResult.Similarity}");
```

Example output:

```json
"ComparisonType": "Exact"
"IsMatch": true
"Similarity": 1.0

```

### Intersecting Names
Check if the given name object intersects with the current name object:

```csharp
var name1 = new Name("John Jacob", "Doe Smith");
var name2 = new Name("John", "Smith");

var intersectsResult = name1.Intersects(name2);

Console.WriteLine($"Comparison Type: {intersectsResult.ComparisonType}");
Console.WriteLine($"Intersects: {intersectsResult.IsMatch}");
Console.WriteLine($"Similarity: {intersectsResult.Similarity}");
```

Example output:

```json
"ComparisonType": "Exact"
"IsMatch": true
"Similarity": 0.5

```

### Comparison Results
Each of the methods above returns a collection of ComparisonResult objects. Each ComparisonResult contains the comparison type, whether the names match, and the similarity score between the names.

To iterate through the results and print the comparison type, match status, and similarity:

```csharp
foreach (var result in matches)
{
    Console.WriteLine($"Comparison Type: {result.ComparisonType}");
    Console.WriteLine($"Is Match: {result.IsMatch}");
    Console.WriteLine($"Similarity: {result.Similarity}");
}
```

### Similarity Score

The similarity score is calculated for each comparison between two names, providing a quantitative measure of how similar the names are. The score ranges from 0.0 to 1.0, where 0.0 represents no similarity and 1.0 indicates an exact match. The similarity score takes into account both the matching characters and the length of the names being compared.

Here's a high-level explanation of how the similarity score is calculated:

1. Compare the two names character by character.
2. If the characters match, add 1 to the matching character count.
3. Calculate the maximum possible matching character count based on the length of the names.
4. Divide the matching character count by the maximum possible matching character count to get the similarity score.

The similarity score can be utilized to filter or rank results based on how closely the names match. For example, you can use the similarity score to show only the names with a similarity score above a certain threshold or to sort the results by similarity score in descending order.

Example:

```csharp
var name1 = new Name("John Jacob", "Doe Smith");
var name2 = new Name("Jon Jacob", "Doe Smyth");

var matches = name1.Matches(name2);

foreach (var result in matches)
{
    Console.WriteLine($"Comparison Type: {result.ComparisonType}");
    Console.WriteLine($"Is Match: {result.IsMatch}");
    Console.WriteLine($"Similarity: {result.Similarity}");
}

// Filtering results based on similarity score
var similarityThreshold = 0.8;
var filteredResults = matches.Where(result => result.Similarity >= similarityThreshold);

// Sorting results by similarity score in descending order
var sortedResults = matches.OrderByDescending(result => result.Similarity);
```

## Additional Features
The library also includes:

- Methods for generating full names, tokenized names, and checking if a name is contained within another name
- A collection of common name suffixes

## Installation
This library is available as a NuGet package. To install, use the following command:

Using the .NET CLI:
```csharp
dotnet add package NameComparisonToolkit
```

Using the Package Manager Console:
```powershell
Install-Package NameComparisonToolkit
```

## License
This library is released under the MIT License.
