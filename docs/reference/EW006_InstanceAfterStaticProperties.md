# EW006: Instance properties should be placed after static properties

## Rule Description

Properties that are static implicitly tell the reader that they do not depend on the instance's state. Therefore, it makes sense to logically separate instance and static properties.
Furthermore, static properties should be above instance properties, since they are usually treated similarly to constants.

## How to fix violations

Move all the instance properties after the last static property.

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
class SomeClass
{
    static int Static1 { get; }

    int Instance1 { get; } // Bad { Move instance property 'Static1' after all the static properties }
    //  ^^^^^^^^^

    static int Static2 { get; }

    int Instance2 { get; }
}
```