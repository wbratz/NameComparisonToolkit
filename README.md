# Name Comparison Toolkit

## Description
This toolkit provides a name object, and multiple name matching methods to compare two names.
All comparison are case insensitive and culture insensitive

## How to use
Once the package is installed create the name objects you would like to compare, then run {name}.Matches({otherName}, {ComparisonType.Type}) if no comparison type is given ExactMatchIgnoreCase is used
Multiple constructors are provided to support as many or as few name parts as desired

## Example
var name1 = new Name("John", "Adam", "Smith", "Jr.");
var name2 = new Name("John", "Adam", "Smith", "Jr.");
name2.Matches(name1)
name2.Matches(name1, ComparisonType.LastNameIgnoreCase);
