# TODO

## Rules

- [x] EW001: Type name contains `Helper`
- [x] EW002: Local functions should be placed at the end of a method
- [x] EW003: Use `is {}` and `is not null` as null-checks (instead of `!= null`).
- [x] EW004: Static methods are prefered to be after instance ones.
- [x] EW005: Static fields should be placed before instance ones.
- [x] EW006: Static properties should be placed before instance ones.

- [ ] EW007: Ordering of class/struct members should be followed.
- [ ] EW008: Each category of members should be ordered from higher to lower accessibility (public>protected internal>internal>protected>private protected>private).

- [ ] EW009: When using an arrow method, the `=>` token must be on the same line as the declaration and the expression body should be on the following.
- [ ] EW010: `if/else if/else` should be used if every branch ends with a `return` statement. Exception is made for input validation.
- [ ] EW011: Var pattern `is var o` can be used only where variable declarations would require additional nesting.
- [ ] EW012: Var pattern `o is { P: var p }` can be used only where `o` can be `null` and `p` is used at least 3 times.
- [ ] EW013: Each compiler directive outside method body (namely `#if`/`#endif`) should be preceded and followed by an empty line.
- [ ] EW014: Single variable lambdas should use `x` as the variable name
- [ ] EW015: Indentation of operators `&&`, `||`, etc.
- [ ] EW016: Groups of members must be separated by an empty line
- [ ] EW017: Dot before an invication is placed at the beginning of a line.
- [ ] EW018: The comma separating arguments is placed at the end of a line.
- [ ] EW019: Field and property initializations are done directly in the member declaration instead of in a constructor.
- [ ] EW020: ???.

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
