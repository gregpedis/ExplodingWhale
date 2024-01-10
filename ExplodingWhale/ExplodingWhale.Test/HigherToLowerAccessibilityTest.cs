using ExplodingWhale.Rules;
using System;

namespace ExplodingWhale.Test;

[TestClass]
public class HigherToLowerAccessibilityTest
{
    [TestMethod]
    public void HigherToLowerAccessibility_NoIssues() =>
        AnalyzerVerifier.Verify<HigherToLowerAccessibility>("""
            public class Empty { }

            public class Simple
            {
                public int @public;
                protected internal int @protected_internal;
                protected int @protected;
                internal int @internal;
                private protected int @private_protected;
                private int @private;
            }

            public class Mixed_OutOfOrder
            {
                private event EventHandler e;

                public int PublicMethod(int x) => 42;

                protected internal Mixed_OutOfOrder() {}

                public class PublicClass {}

                public int @public;

                protected internal int @protected_internal;

                internal int @internal_property { get => 42; }

                protected int @protected;

                private Mixed_OutOfOrder(int x) {}

                internal int @internal;

                private protected int @private_protected;

                private int @private;

                int @implicit_private;

                internal class InternalClass {}

                private int @private_property { get; set; }

                protected int ProtectedMethod(int x) => 42;
            }
            """);

    [TestMethod]
    public void HigherToLowerAccessibility_ShouldRaise() =>
        AnalyzerVerifier.Verify<HigherToLowerAccessibility>("""
            public class Simple
            {
                int x0;

                public int x1; // Bad

                protected internal int x2; // Bad { Move member 'x2' above all members of the same kind with less accessibility. }

                internal int x3; // Bad

                protected int x4; // Bad

                private protected int x5; // Bad

                private int x6;
            } 
            """);
}
