# EW003: Use 'is (not)' to compare with null

## Rule Description

In general, when checking for null, both == null and is null are often used interchangeably. However, if you want to emphasize null-checking or if you are working in a pattern-matching context, using is null can be more idiomatic and expressive. Additionally, is null may be more suitable when working with patterns, like switch expressions, where it can be used to match a specific pattern.

## How to fix violations

Replace `==` with `is` and `!=` with `is not`.

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
public int Method(Thingy arg)
{
    // Validating arg
    if (arg == null) // Bad { Replace '==' with 'is' when comparing with null }
    {
        // throw
    }
    // DoWork
}
```