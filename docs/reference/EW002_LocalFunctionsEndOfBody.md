# EW002: Local Functions should be placed at the end of the body

## Rule Description

Local functions help encapsulate functionality and make logic of a body more readable.
To make it simple for the reader, there should be not intermingling of logic and extracted local functionality. Therefore, all the logic of the method/constructor/etc should preceed local functions.

## How to fix violations

Move the local functions at the end of the body.

## When to suppress warnings

Not applicable.

## Example of a violation

```csharp
public int Method()
{
    var x = 1;
    return AddOne(x);

    int AddOne(int x) => x+1;   // Bad { Move 'AddOne' at the end of the body. }
    //  ^^^^^^

    var shouldNotBeHere = 42;
}
```