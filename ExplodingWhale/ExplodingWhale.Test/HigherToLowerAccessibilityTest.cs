using ExplodingWhale.Rules;

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
                internal int @internal;
                protected int @protected;
                private protected int @private_protected;
                private int @private;
            }

            public class Mixed_OutOfOrder
            {
                static Mixed_OutOfOrder() {}

                private event EventHandler e;

                public int PublicMethod(int x) => 42;

                protected internal Mixed_OutOfOrder() {}

                public class PublicClass {}

                public int @public;

                protected internal int @protected_internal;

                internal int @internal_property { get => 42; }

                internal int @internal;

                protected int @protected;

                private Mixed_OutOfOrder(int x) {}

                private protected int @private_protected;

                private int @private;

                int @implicit_private;

                internal class InternalClass {}

                private int @private_property { get; set; }

                private protected static int PrivateProtectedStaticMethod(int x) => 42;

                protected int ProtectedMethod(int x) => 42;

                private static int PrivateStaticMethod(int x) => 42;
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

            public class Mixed
            {
                ~Mixed() {} // Destructors cannot have access modifiers

                private Mixed(int x) {}
                protected internal Mixed() {} // Bad

                static Mixed() {} // Static constructor can only be parameterless and without access modifiers

                private event EventHandler e1;
                public event EventHandler e2; // Bad

                protected int ProtectedMethod(int x) => 42;
                public int PublicMethod(int x) => 42; // Bad

                internal class InternalClass {}
                protected internal class PublicClass {} // Bad

                public int @public;
                protected internal int @protected_internal;
                private int @private;
                protected int @protected; // Bad
                private protected int @private_protected; // Bad
                int @implicit_private;
                internal int @internal; // Bad

                internal int @internal_property { get => 42; }
                private int @private_property { get; set; }
                public int @public_property { get; set; } // Bad

                private static int PrivateStaticMethod(int x) => 42;
                private protected static int PrivateProtectedStaticMethod(int x) => 42; // Bad

                public static Mixed operator +(Mixed x) => default; // User operators must be public and static
            }
            """);
}
