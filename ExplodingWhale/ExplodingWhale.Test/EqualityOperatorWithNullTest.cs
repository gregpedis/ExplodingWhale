﻿using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class EqualityOperatorWithNullTest
{
    [TestMethod]
    public void EqualityOperatorWithNull_NoIssues() =>
        AnalyzerVerifier.Verify<EqualityOperatorWithNull>("""
            public class SomeClass
            {
                public int Method(SomeClass thingy)
                {
                    _ = thingy is null;
                    _ = thingy is not null;

                    _ = null == null;
                    _ = null != null;
                }
            }
            """);

    [TestMethod]
    public void EqualityOperatorWithNull_ShouldRaise() =>
        AnalyzerVerifier.Verify<EqualityOperatorWithNull>("""
            public class SomeClass
            {
                public int Method(SomeClass thingy)
                {
                    _ = thingy == null;             // Bad {Replace '==' with 'is' when comparing with null.}
                    _ = thingy != null;             // Bad

                    _ = thingy == null ? 42 : 0;    // Bad
                    //  ^^^^^^^^^^^^^^

                    _ = thingy != ((((null))));     // Bad
                    //  ^^^^^^^^^^^^^^^^^^^^^^

                    _ = null == thingy;             // Bad
                    _ = (null) != thingy;           // Bad {Replace '!=' with 'is not' and reverse the operands when comparing with null.}
                }
            }
            """);
}
