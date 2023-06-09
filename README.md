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

Mext, create a `Name` object for each name you want to compare:

```csharp
var name1 = new Name("John", "Doe");
var name2 = new Name("John", "Smith");
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
// ...
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
// ...
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
// ...
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
// ...
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
