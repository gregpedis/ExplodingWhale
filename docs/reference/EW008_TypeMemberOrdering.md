# EW008: Ordering of type members should be followed

## Rule Description

Type members should be ordered based on what they are, since their kind has semantic meaning. This means all the fields, properties, methods, etc should be grouped and ordered respectively.

For example, all field should be found before all properties.

This makes it easier for the reader to track what the type wants to achieve, while also simplifying future refactorings.

## How to fix violations

Order the members of the type as follows:

1. Constants
1. Nested enum declarations
1. Fields
1. Abstract members
1. Properties
1. Constructors
1. Methods
1. Private nested classes

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
class SomeClass
{
    int Double(x) => x*2;

    int field1; // Bad { Move this member above all methods. }
    //  ^^^^^^
}
```