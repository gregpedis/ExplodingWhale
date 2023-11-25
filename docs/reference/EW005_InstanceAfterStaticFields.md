# EW005: Instance fields should be placed after static fields

## Rule Description

Fields that are static implicitly tell the reader that they do not depend on the instance's state. Therefore, it makes sense to logically separate instance and static fields.
Furthermore, static fields should be above instance fields, since they are usually treated similarly to constants.

## How to fix violations

Move all the instance fields after the last static field.

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
class SomeClass
{
    static int Static1;

    int Instance1; // Bad { Move instance field 'Static1' after all the static fields }
    //  ^^^^^^^^^

    static int Static2;

    int Instance2;
}
```