# NameComparisonToolkit

A powerful and flexible .NET library for comparing names in a variety of ways. It allows you to compare names using different comparison types, as well as providing useful methods for tokenizing, containing, and intersecting names.

## Table of Contents

- [Features](#features)
- [Usage](#usage)
  - [Comparing Names](#comparing-names)
  - [Containing Names](#containing-names)
  - [Intersecting Names](#intersecting-names)
  - [Comparison Results](#comparison-results)
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

First, create a `Name` object for each name you want to compare:

```csharp
var name1 = new Name("John", "Doe");
var name2 = new Name("John", "Smith");
```

### Comparing Names
Compare the two names using all available comparison types:

```csharp
var matches = name1.Matches(name2);
```

Compare the two names ignoring the order of the name parts:
```csharp
var matchesIgnoreOrder = name1.MatchesIgnoreOrder(name2);
```

### Containing Names
Check if the current name object is contained within the given name string:

```csharp
var contains = name1.Contains("John Doe");
```

### Intersecting Names
Check if the given name object intersects with the current name object:

```csharp
var intersects = name1.Intersects(name2);
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
