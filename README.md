# Exploding Whale

![Exploding Whale Logo](./logo.jpg)

Some coding style analyzers, based on the [coding style](https://github.com/SonarSource/sonar-dotnet/blob/master/docs/coding-style.md) of `sonar-dotnet`.

## Rules

- [x] EW001: Type name contains `Helper`
- [x] EW002: Local functions should be placed at the end of a method
- [x] EW003: Use `is {}` and `is not null` as null-checks (instead of `!= null`).
- [ ] EW004: Single variable lambdas should use `x` as the variable name
- [ ] EW005: Var pattern `is var o` can be used only where variable declarations would require additional nesting.
- [ ] EW006: Var pattern `o is { P: var p }` can be used only where `o` can be `null` and `p` is used at least 3 times.
- [ ] EW007: Each compiler directive outside method body (namely `#if`/`#endif`) should be preceded and followed by an empty line.
- [ ] EW008: Field and property initializations are done directly in the member declaration instead of in a constructor.
- [ ] EW009: When using an arrow method, the `=>` token must be on the same line as the declaration and the expression body should be on the following.
- [ ] EW010: `if/else if/else` should be used if every branch ends with a `return` statement. Exception is made for input validation.
- [ ] EW011: Ordering of class/struct members should be followed.
- [ ] EW012: Each category of members should be ordered from higher to lower accessibility (public>protected internal>internal>protected>private protected>private).
- [ ] EW013: Static fields and properties should be placed before instance ones.
- [ ] EW014: Static methods are prefered to be after instance ones.
- [ ] EW015: Indentation of operators `&&`, `||`, etc.
- [ ] EW016: Groups of members must be separated by an empty line
- [ ] EW017: Dot before an invication is placed at the beginning of a line.
- [ ] EW018: The comma separating arguments is placed at the end of a line.
- [ ] EW019: ???.

## Boring stuff

- Add package signing
- Add social media preview when repo is public
- Fix package logo URL in README.md to full github url for nuget.org when repo is public
- Maybe do codefixes?
<!-- - Make a list of rules -->
<!-- - Add readme for package -->
<!-- - Cleanup nuspec -->
<!-- - Add a rule wiki -->
<!-- - Connect rules with descriptions -->
<!-- - Make a UT framework -->
<!-- - Setup some analysis framework -->
<!-- - Make a codefix+testing framework -->
