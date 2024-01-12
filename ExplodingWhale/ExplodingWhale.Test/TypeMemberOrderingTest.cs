using ExplodingWhale.Rules;

namespace ExplodingWhale.Test;

[TestClass]
public class TypeMemberOrderingTest
{
    [TestMethod]
    public void TypeMemberOrdering_NoIssues() =>
        AnalyzerVerifier.Verify<TypeMemberOrdering>("""
            public class Empty { }

            public abstract class Mixed
            {
                const int constField = 42;

                enum SomeEnum { One, Two }

                int field = 50;

                protected abstract int AbstractProperty { get; }

                int NormalProperty { get; set; }

                public Mixed() { }

                void Method() { }

                class Nested { }
            }
            """);

    [TestMethod]
    public void TypeMemberOrdering_ShouldRaise() =>
        AnalyzerVerifier.Verify<TypeMemberOrdering>("""
            public abstract class Mixed
            {
                const int constField = 42;

                enum SomeEnum { One, Two }

                const int constField_bad = 42; // Bad { Move this member above all nested enums. }
                //        ^^^^^^^^^^^^^^

                int field = 50;

                protected abstract int AbstractProperty { get; }

                int NormalProperty { get; set; }

                public Mixed() { }

                int NormalProperty_bad { get; set; } // Bad { Move this member above all constructors. }

                void Method() { }

                public Mixed(int x) { } // Bad { Move this member above all methods. }

                class Nested { }

                void Method_bad() { } // Bad { Move this member above all nested types. }

                protected abstract int AbstractProperty_bad { get; }  // Bad { Move this member above all properties. }

                int field_bad = 50; // Bad { Move this member above all abstract members. }

                enum SomeEnum_bad { One, Two } // Bad { Move this member above all fields. }
            }
            """);
}
