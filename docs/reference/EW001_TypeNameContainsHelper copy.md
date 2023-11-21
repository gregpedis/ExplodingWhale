# EW001: Type Names should not contain "Helper"

## Rule Description

Types should have strong, single purposes.

`Helper` is not a very descriptive word, meaning the person who will read the code at a later time will not understand what the purpose of the type is.

## How to fix violations

Rename the type without the word `Helper`.

## When to suppress warnings

If for any reason you cannot rename the type.

## Example of a violation

```csharp
class ThingyHelper { } // Bad
//          ^^^^^^
```