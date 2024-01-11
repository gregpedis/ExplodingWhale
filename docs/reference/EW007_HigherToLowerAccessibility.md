# EW007: Members of the same kind should be ordered from higher to lower accessibility

## Rule Description

Accesibility shows what is exposed and at what level. It is useful to group members of the same kind at the level of exposure, to clearly illustrate the intent of each member.
This rule groups members based on their kind, for example fields, properties, methods, constructors. Furthermore, it also groups the static members separately from the instance ones.

## How to fix violations

Order the static/instance members of the same kind from higher to lower accessibility based on their access modifiers:
- public
- protected internal
- internal
- protected
- private protected
- private (or no access modifiers, implicitly private)

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
class SomeClass
{
    int field1;
    protected int field2; // Bad { Move member 'field2' above all members of the same kind with less accessibility. }
    //            ^^^^^^
}
```