# TODO

## Rules

- [ ] Groups of members must be separated by an empty line.
- [ ] Each compiler directive outside method body (namely `#if`/`#endif`) should be preceded and followed by an empty line.
- [ ] When using an arrow method, the `=>` token must be on the same line as the declaration and the expression body should be on the following.

- [ ] `if/else if/else` should be used if every branch ends with a `return` statement. Exception is made for input validation.
- [ ] Field and property initializations are done directly in the member declaration instead of in a constructor.

- [ ] Var pattern `is var o` can be used only where variable declarations would require additional nesting.
- [ ] Var pattern `o is { P: var p }` can be used only where `o` can be `null` and `p` is used at least 3 times.

- [ ] Single variable lambdas should use `x` as the variable name
- [ ] Indentation of operators `&&`, `||`, etc.
- [ ] Dot before an invocation is placed at the beginning of a line.
- [ ] The comma separating arguments is placed at the end of a line.
- [ ] ???.

## Boring stuff

- Add package signing
- Maybe do codefixes?

<!-- - Make a list of rules -->
<!-- - Add readme for package -->
<!-- - Cleanup nuspec -->
<!-- - Add a rule wiki -->
<!-- - Connect rules with descriptions -->
<!-- - Make a UT framework -->
<!-- - Setup some analysis framework -->
<!-- - Make a codefix+testing framework -->
